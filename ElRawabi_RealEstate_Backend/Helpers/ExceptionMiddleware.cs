using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

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
                _logger.LogError(ex, $"خطأ في المسار: {context.Request.Path}");

                context.Response.ContentType = "application/json";

                var (statusCode, message) = GetErrorDetails(ex);
                context.Response.StatusCode = statusCode;

                var response = new
                {
                    StatusCode = statusCode,
                    Message = message,
                    Path = context.Request.Path.ToString(),
                    StackTrace = _env.IsDevelopment() ? ex.StackTrace : null
                };

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
            }
        }

        private static (int statusCode, string message) GetErrorDetails(Exception ex)
        {
            // Unique Constraint - بيجي من EF Core
            if (ex is DbUpdateException dbEx)
            {
                var inner = dbEx.InnerException?.Message ?? "";

                if (inner.Contains("Email") || inner.Contains("IX_Users_Email") || inner.Contains("IX_Buyers_Email"))
                    return (409, "البريد الإلكتروني مسجل مسبقاً");

                if (inner.Contains("PhoneNumber") || inner.Contains("IX_Buyers_PhoneNumber"))
                    return (409, "رقم الهاتف مسجل مسبقاً");

                if (inner.Contains("NationalId") || inner.Contains("IX_Buyers_NationalId"))
                    return (409, "رقم الهوية مسجل مسبقاً");

                if (inner.Contains("IX_Projects_Name"))
                    return (409, "اسم المشروع مسجل مسبقاً");

                if (inner.Contains("IX_Units_UnitNumber"))
                    return (409, "رقم الوحدة مسجل مسبقاً في هذا الدور");

                if (inner.Contains("IX_Roles_RoleName"))
                    return (409, "اسم الدور مسجل مسبقاً");

                if (inner.Contains("unique") || inner.Contains("UNIQUE") || inner.Contains("duplicate"))
                    return (409, "البيانات المدخلة مسجلة مسبقاً");

                return (400, "خطأ في حفظ البيانات");
            }

            if (ex is InvalidOperationException)
                return (400, ex.Message);

            if (ex is UnauthorizedAccessException)
                return (401, "غير مصرح لك بهذا الإجراء");

            if (ex is KeyNotFoundException)
                return (404, "العنصر المطلوب غير موجود");

            return (500, "حدث خطأ في الخادم، يرجى المحاولة مرة أخرى");
        }
    }
}