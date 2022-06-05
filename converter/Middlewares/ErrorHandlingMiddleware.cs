using System.Net;

namespace converter.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly RequestDelegate _requestDelegate;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, RequestDelegate requestDelegate)
        {
            _logger = logger;
            _requestDelegate = requestDelegate;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception ex)
            {
                await WriteLogAsync(context, ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        public async Task WriteLogAsync(HttpContext context, string message, HttpStatusCode statusCode)
        {
            _logger.LogError(message);

            HttpResponse response = context.Response;

            response.StatusCode = (int)statusCode;

            await response.WriteAsync(message);
        }
    }
}
