

namespace TaskManagement.Middlewares
{
    public class RequestLoggingMiddlewares
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddlewares(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Console.WriteLine($"[{DateTime.UtcNow}] {context.Request.Method} {context.Request.Path}");
            await _next(context);
        }
    }
}
