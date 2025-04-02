using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;

namespace DekatMe.Api.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly RequestLoggingOptions _options;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger, RequestLoggingOptions? options = null)
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
            _options = options ?? new RequestLoggingOptions();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Don't log if the path matches any of the excluded paths
            if (_options.ExcludedPaths.Any(path => context.Request.Path.StartsWithSegments(path)))
            {
                await _next(context);
                return;
            }

            var correlationId = GetOrCreateCorrelationId(context);
            var requestTime = DateTime.UtcNow;
            var sw = Stopwatch.StartNew();
            
            try
            {
                var originalBodyStream = context.Response.Body;
                await using var responseBody = _recyclableMemoryStreamManager.GetStream();
                context.Response.Body = responseBody;
                
                string requestBody = await GetRequestBody(context.Request);
                
                // Only log request if it's under the max size
                if (requestBody.Length <= _options.MaxBodyLogSize)
                {
                    LogRequest(context, correlationId, requestTime, requestBody);
                }
                else
                {
                    LogRequestHeaders(context, correlationId, requestTime);
                    _logger.LogInformation("Request body truncated (size: {RequestBodySize})", requestBody.Length);
                }

                await _next(context);
                
                sw.Stop();
                
                string responseBody = await GetResponseBody(context.Response);
                
                // Only log response if it's under the max size
                if (responseBody.Length <= _options.MaxBodyLogSize)
                {
                    LogResponse(context, correlationId, sw.ElapsedMilliseconds, responseBody);
                }
                else
                {
                    LogResponseHeaders(context, correlationId, sw.ElapsedMilliseconds);
                    _logger.LogInformation("Response body truncated (size: {ResponseBodySize})", responseBody.Length);
                }
                
                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (Exception ex)
            {
                sw.Stop();
                _logger.LogError(ex, "An error occurred during request logging. Request: {Method} {Path}", 
                    context.Request.Method, context.Request.Path);
                throw;
            }
        }

        private string GetOrCreateCorrelationId(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(_options.CorrelationIdHeader, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers[_options.CorrelationIdHeader] = correlationId;
            }
            
            // Ensure the correlation ID is included in the response
            context.Response.Headers[_options.CorrelationIdHeader] = correlationId;
            
            return correlationId;
        }

        private async Task<string> GetRequestBody(HttpRequest request)
        {
            // If we should skip logging the body based on content type
            if (_options.ExcludedContentTypes.Any(ct => request.ContentType?.StartsWith(ct) == true))
            {
                return "[Content type excluded from logging]";
            }

            request.EnableBuffering();
            
            var buffer = new byte[_options.MaxBodyLogSize];
            var bytesRead = await request.Body.ReadAsync(buffer, 0, buffer.Length);
            
            request.Body.Position = 0; // Reset position for future reading
            
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }

        private async Task<string> GetResponseBody(HttpResponse response)
        {
            // If we should skip logging the body based on content type
            if (_options.ExcludedContentTypes.Any(ct => response.ContentType?.StartsWith(ct) == true))
            {
                return "[Content type excluded from logging]";
            }
            
            response.Body.Position = 0;
            
            var buffer = new byte[_options.MaxBodyLogSize];
            var bytesRead = await response.Body.ReadAsync(buffer, 0, buffer.Length);
            
            response.Body.Position = 0; // Reset position for future reading
            
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }

        private void LogRequest(HttpContext context, string correlationId, DateTime requestTime, string body)
        {
            var request = context.Request;
            
            _logger.LogInformation(
                "Request {CorrelationId} [{RequestTime}]: {Method} {Scheme}://{Host}{Path}{QueryString} {Protocol} " +
                "User-Agent: {UserAgent} Content-Type: {ContentType} Content-Length: {ContentLength}",
                correlationId,
                requestTime.ToString("u"),
                request.Method,
                request.Scheme,
                request.Host,
                request.Path,
                request.QueryString,
                request.Protocol,
                request.Headers["User-Agent"],
                request.ContentType,
                request.ContentLength);
            
            if (!string.IsNullOrEmpty(body) && _options.IncludeRequestBody)
            {
                _logger.LogInformation("Request Body {CorrelationId}: {Body}", correlationId, body);
            }
        }

        private void LogRequestHeaders(HttpContext context, string correlationId, DateTime requestTime)
        {
            var request = context.Request;
            
            _logger.LogInformation(
                "Request {CorrelationId} [{RequestTime}]: {Method} {Scheme}://{Host}{Path}{QueryString} {Protocol}",
                correlationId,
                requestTime.ToString("u"),
                request.Method,
                request.Scheme,
                request.Host,
                request.Path,
                request.QueryString,
                request.Protocol);
                
            if (_options.IncludeHeaders)
            {
                foreach (var header in request.Headers)
                {
                    if (!_options.ExcludedHeaders.Contains(header.Key))
                    {
                        _logger.LogInformation("Request Header {CorrelationId} - {Key}: {Value}", 
                            correlationId, header.Key, header.Value);
                    }
                }
            }
        }

        private void LogResponse(HttpContext context, string correlationId, long elapsedMs, string body)
        {
            var response = context.Response;
            
            _logger.LogInformation(
                "Response {CorrelationId}: {StatusCode} {ReasonPhrase} (took {ElapsedMs}ms) Content-Type: {ContentType} Content-Length: {ContentLength}",
                correlationId,
                response.StatusCode,
                GetReasonPhrase(response.StatusCode),
                elapsedMs,
                response.ContentType,
                response.ContentLength);
            
            if (!string.IsNullOrEmpty(body) && _options.IncludeResponseBody)
            {
                _logger.LogInformation("Response Body {CorrelationId}: {Body}", correlationId, body);
            }
        }

        private void LogResponseHeaders(HttpContext context, string correlationId, long elapsedMs)
        {
            var response = context.Response;
            
            _logger.LogInformation(
                "Response {CorrelationId}: {StatusCode} {ReasonPhrase} (took {ElapsedMs}ms)",
                correlationId,
                response.StatusCode,
                GetReasonPhrase(response.StatusCode),
                elapsedMs);
                
            if (_options.IncludeHeaders)
            {
                foreach (var header in response.Headers)
                {
                    if (!_options.ExcludedHeaders.Contains(header.Key))
                    {
                        _logger.LogInformation("Response Header {CorrelationId} - {Key}: {Value}", 
                            correlationId, header.Key, header.Value);
                    }
                }
            }
        }

        private string GetReasonPhrase(int statusCode)
        {
            return statusCode switch
            {
                StatusCodes.Status100Continue => "Continue",
                StatusCodes.Status101SwitchingProtocols => "Switching Protocols",
                StatusCodes.Status200OK => "OK",
                StatusCodes.Status201Created => "Created",
                StatusCodes.Status202Accepted => "Accepted",
                StatusCodes.Status204NoContent => "No Content",
                StatusCodes.Status400BadRequest => "Bad Request",
                StatusCodes.Status401Unauthorized => "Unauthorized",
                StatusCodes.Status403Forbidden => "Forbidden",
                StatusCodes.Status404NotFound => "Not Found",
                StatusCodes.Status405MethodNotAllowed => "Method Not Allowed",
                StatusCodes.Status409Conflict => "Conflict",
                StatusCodes.Status500InternalServerError => "Internal Server Error",
                StatusCodes.Status501NotImplemented => "Not Implemented",
                StatusCodes.Status502BadGateway => "Bad Gateway",
                StatusCodes.Status503ServiceUnavailable => "Service Unavailable",
                _ => string.Empty
            };
        }
    }

    public class RequestLoggingOptions
    {
        public string CorrelationIdHeader { get; set; } = "X-Correlation-ID";
        public int MaxBodyLogSize { get; set; } = 4096;
        public bool IncludeRequestBody { get; set; } = true;
        public bool IncludeResponseBody { get; set; } = true;
        public bool IncludeHeaders { get; set; } = true;
        public List<string> ExcludedPaths { get; set; } = new List<string> { "/health", "/metrics" };
        public List<string> ExcludedContentTypes { get; set; } = new List<string> { "image/", "audio/", "video/" };
        public List<string> ExcludedHeaders { get; set; } = new List<string> { "Authorization", "Cookie", "Set-Cookie" };
    }

    // Extension method used to add the middleware to the HTTP request pipeline
    public static class RequestLoggingMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>();
        }

        public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder builder, RequestLoggingOptions options)
        {
            return builder.UseMiddleware<RequestLoggingMiddleware>(options);
        }
    }
}
