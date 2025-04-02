using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DekatMe.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred during the request execution");

            var statusCode = HttpStatusCode.InternalServerError;
            var errorResponse = new ErrorResponse
            {
                TraceId = Guid.NewGuid().ToString(),
                Message = "An unexpected error occurred"
            };

            // Customize response based on exception type
            switch (exception)
            {
                case BadHttpRequestException badRequestEx:
                    statusCode = HttpStatusCode.BadRequest;
                    errorResponse.Message = "Invalid request format";
                    errorResponse.Details = badRequestEx.Message;
                    break;

                case UnauthorizedAccessException unauthorizedEx:
                    statusCode = HttpStatusCode.Unauthorized;
                    errorResponse.Message = "Authentication required";
                    errorResponse.Details = unauthorizedEx.Message;
                    break;

                case KeyNotFoundException notFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    errorResponse.Message = "The requested resource was not found";
                    errorResponse.Details = notFoundEx.Message;
                    break;

                case ArgumentException argEx:
                    statusCode = HttpStatusCode.BadRequest;
                    errorResponse.Message = "Invalid argument provided";
                    errorResponse.Details = argEx.Message;
                    break;

                case InvalidOperationException invalidOpEx:
                    statusCode = HttpStatusCode.BadRequest;
                    errorResponse.Message = "The requested operation is invalid";
                    errorResponse.Details = invalidOpEx.Message;
                    break;

                case ApplicationException appEx:
                    statusCode = HttpStatusCode.BadRequest;
                    errorResponse.Message = appEx.Message;
                    break;

                case ValidationException validationEx:
                    statusCode = HttpStatusCode.BadRequest;
                    errorResponse.Message = "Validation failed";
                    errorResponse.ValidationErrors = validationEx.ValidationErrors;
                    break;

                case JsonException jsonEx:
                    statusCode = HttpStatusCode.BadRequest;
                    errorResponse.Message = "Invalid JSON format";
                    errorResponse.Details = jsonEx.Message;
                    break;

                default:
                    // Only include detailed exception information in development
                    if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
                    {
                        errorResponse.Details = exception.Message;
                        errorResponse.StackTrace = exception.StackTrace;
                    }
                    break;
            }

            _logger.LogError("Error {TraceId}: {Message}", errorResponse.TraceId, errorResponse.Message);

            context.Response.StatusCode = (int)statusCode;
            context.Response.ContentType = "application/json";

            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(errorResponse, serializerOptions);
            await context.Response.WriteAsync(json);
        }
    }

    public class ErrorResponse
    {
        public string TraceId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public string? StackTrace { get; set; }
        public IDictionary<string, string[]>? ValidationErrors { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> ValidationErrors { get; }

        public ValidationException(string message, IDictionary<string, string[]> validationErrors)
            : base(message)
        {
            ValidationErrors = validationErrors;
        }

        public ValidationException(string message, string propertyName, string errorMessage)
            : base(message)
        {
            ValidationErrors = new Dictionary<string, string[]>
            {
                { propertyName, new[] { errorMessage } }
            };
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline
    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandlingMiddleware>();
        }
    }
}
