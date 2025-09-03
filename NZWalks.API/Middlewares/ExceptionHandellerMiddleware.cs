using System.Net;

namespace NZWalks.API.Middlewares
{
    public class ExceptionHandellerMiddleware
    {
        private readonly ILogger<ExceptionHandellerMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionHandellerMiddleware(ILogger<ExceptionHandellerMiddleware> logger, RequestDelegate next)
        {
            _logger = logger;
            _next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var errorId = Guid.NewGuid();
                // log the exception
                _logger.LogError(ex, $"{errorId} :  {ex.Message}");

                // Return a custome error response
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                httpContext.Response.ContentType = "application/json";

                var error = new
                {
                    Id = errorId,
                    ErrorMessage = "Something went wrong ! We are looking into this"
                };

                await httpContext.Response.WriteAsJsonAsync(error);

            }
        }
    }
}
