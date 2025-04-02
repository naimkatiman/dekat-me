using System.Collections.Concurrent;
using System.Net;
using System.Text.Json;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DekatMe.Api.Middleware
{
    public class ApiRateLimitingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiRateLimitingMiddleware> _logger;
        private readonly RateLimitingOptions _options;
        private readonly ConcurrentDictionary<string, RateLimitingState> _clientStates;
        private readonly IMemoryCache _cache;

        public ApiRateLimitingMiddleware(
            RequestDelegate next,
            ILogger<ApiRateLimitingMiddleware> logger,
            IOptions<RateLimitingOptions> options,
            IMemoryCache cache)
        {
            _next = next;
            _logger = logger;
            _options = options.Value;
            _clientStates = new ConcurrentDictionary<string, RateLimitingState>();
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Skip rate limiting for excluded paths
            if (_options.ExcludedPaths.Any(path => context.Request.Path.StartsWithSegments(path)))
            {
                await _next(context);
                return;
            }

            // Get client identifier based on the options
            var clientId = GetClientIdentifier(context);

            // Try to get or create rate limiter for this client
            var rateLimiterState = _clientStates.GetOrAdd(clientId, _ => new RateLimitingState(
                new TokenBucketRateLimiter(new TokenBucketRateLimiterOptions
                {
                    TokenLimit = _options.TokenLimit,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                    QueueLimit = _options.QueueLimit,
                    ReplenishmentPeriod = TimeSpan.FromSeconds(_options.ReplenishmentPeriodSeconds),
                    TokensPerPeriod = _options.TokensPerPeriod,
                    AutoReplenishment = true
                })));

            // Check if the client is in the allowed list
            if (_options.WhitelistedClients.Contains(clientId))
            {
                await _next(context);
                return;
            }

            // Try to acquire a permit from the rate limiter
            using RateLimitLease lease = await rateLimiterState.RateLimiter.AcquireAsync(
                permitCount: 1,
                cancellationToken: context.RequestAborted);

            if (lease.IsAcquired)
            {
                // We got a permit, continue with the request
                AddRateLimitHeaders(context, rateLimiterState, lease, clientId);
                await _next(context);
            }
            else
            {
                // We did not get a permit, return a rate limit response
                AddRateLimitHeaders(context, rateLimiterState, lease, clientId);
                await HandleRateLimitExceededResponse(context, lease, clientId);
            }
        }

        private string GetClientIdentifier(HttpContext context)
        {
            return _options.ClientIdSource switch
            {
                ClientIdSource.IpAddress => context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
                ClientIdSource.ApiKey => GetApiKey(context),
                ClientIdSource.UserId => GetUserId(context),
                _ => context.Connection.RemoteIpAddress?.ToString() ?? "unknown"
            };
        }

        private string GetApiKey(HttpContext context)
        {
            // Try to get API key from the authorization header
            if (context.Request.Headers.TryGetValue("Authorization", out var authHeader) &&
                authHeader.ToString().StartsWith("ApiKey ", StringComparison.OrdinalIgnoreCase))
            {
                return authHeader.ToString().Substring(7).Trim();
            }

            // Try to get API key from the query string
            if (context.Request.Query.TryGetValue("api_key", out var apiKey))
            {
                return apiKey.ToString();
            }

            return "unknown";
        }

        private string GetUserId(HttpContext context)
        {
            // Try to get user ID from claims
            var userId = context.User?.Identity?.Name;
            if (!string.IsNullOrEmpty(userId))
            {
                return userId;
            }

            // Fallback to IP address if no user ID found
            return context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        }

        private void AddRateLimitHeaders(HttpContext context, RateLimitingState state, RateLimitLease lease, string clientId)
        {
            // Calculate available tokens
            int availableTokens = state.RateLimiter.GetAvailablePermits();
            
            // Add rate limit information headers
            context.Response.Headers["X-RateLimit-Limit"] = _options.TokenLimit.ToString();
            context.Response.Headers["X-RateLimit-Remaining"] = availableTokens.ToString();
            context.Response.Headers["X-RateLimit-Client-ID"] = clientId;
            
            if (!lease.IsAcquired)
            {
                if (lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
                {
                    context.Response.Headers["Retry-After"] = ((int)retryAfter.TotalSeconds).ToString();
                }
            }
        }

        private async Task HandleRateLimitExceededResponse(HttpContext context, RateLimitLease lease, string clientId)
        {
            _logger.LogWarning("Rate limit exceeded for client {ClientId}", clientId);
            
            context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
            context.Response.ContentType = "application/json";
            
            var retryAfter = lease.TryGetMetadata(MetadataName.RetryAfter, out var timeSpan) 
                ? (int)timeSpan.TotalSeconds 
                : _options.DefaultRetryAfterSeconds;
            
            var rateLimitResponse = new RateLimitResponse
            {
                Status = 429,
                Title = "Too Many Requests",
                Detail = $"API rate limit has been exceeded. Please retry after {retryAfter} seconds.",
                RetryAfter = retryAfter
            };
            
            var jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            
            var jsonResponse = JsonSerializer.Serialize(rateLimitResponse, jsonOptions);
            await context.Response.WriteAsync(jsonResponse);
        }
    }

    public class RateLimitingState
    {
        public TokenBucketRateLimiter RateLimiter { get; }
        public DateTime LastReplenishmentTime { get; set; }
        
        public RateLimitingState(TokenBucketRateLimiter rateLimiter)
        {
            RateLimiter = rateLimiter;
            LastReplenishmentTime = DateTime.UtcNow;
        }
    }

    public class RateLimitResponse
    {
        public int Status { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Detail { get; set; } = string.Empty;
        public int RetryAfter { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class RateLimitingOptions
    {
        public ClientIdSource ClientIdSource { get; set; } = ClientIdSource.IpAddress;
        public int TokenLimit { get; set; } = 100;
        public int TokensPerPeriod { get; set; } = 20;
        public int ReplenishmentPeriodSeconds { get; set; } = 60;
        public int QueueLimit { get; set; } = 2;
        public int DefaultRetryAfterSeconds { get; set; } = 30;
        public List<string> WhitelistedClients { get; set; } = new List<string>();
        public List<string> ExcludedPaths { get; set; } = new List<string> { "/health", "/metrics", "/swagger" };
    }

    public enum ClientIdSource
    {
        IpAddress,
        ApiKey,
        UserId
    }

    // Extension method used to add the middleware to the HTTP request pipeline
    public static class ApiRateLimitingMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiRateLimiting(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiRateLimitingMiddleware>();
        }
    }
}
