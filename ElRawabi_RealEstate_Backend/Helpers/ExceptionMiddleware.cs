using System.Net;
using System.Text.Json;

namespace ElRawabi_RealEstate_Backend.Helpers
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try { await _next(context); }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"خطأ في المسار: {context.Request.Path} | الرسالة: {ex.Message}");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new
                {
                    StatusCode = context.Response.StatusCode,
                    ErrorType = ex.GetType().Name,
                    Message = ex.Message,
                    Path = context.Request.Path.ToString(),

                    StackTrace = _env.IsDevelopment() ? ex.StackTrace : "Contact Admin"
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
            }
        }
    }
}
