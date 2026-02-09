using System.Net;
using System.Text.Json;

namespace policy_management.Filters
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var errorResponse = new ErrorResponse
            {
                TraceId = context.TraceIdentifier
            };

            switch (exception)
            {
                case ArgumentException argEx:
                    errorResponse.ErrorCode = "INVALID_ARGUMENT";
                    errorResponse.Message = argEx.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case InvalidOperationException invOpEx:
                    errorResponse.ErrorCode = "INVALID_OPERATION";
                    errorResponse.Message = invOpEx.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;

                case KeyNotFoundException keyNotFoundEx:
                    errorResponse.ErrorCode = "RESOURCE_NOT_FOUND";
                    errorResponse.Message = keyNotFoundEx.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case UnauthorizedAccessException unauthorizedEx:
                    errorResponse.ErrorCode = "UNAUTHORIZED_ACCESS";
                    errorResponse.Message = "You are not authorized to perform this action";
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case TimeoutException timeoutEx:
                    errorResponse.ErrorCode = "REQUEST_TIMEOUT";
                    errorResponse.Message = "The request timed out. Please try again later";
                    context.Response.StatusCode = (int)HttpStatusCode.RequestTimeout;
                    break;

                default:
                    errorResponse.ErrorCode = "INTERNAL_SERVER_ERROR";
                    errorResponse.Message = "An unexpected error occurred. Please try again later";
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(errorResponse, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(jsonResponse);
        }
    }

    public class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public string TraceId { get; set; }
    }
}
