using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Diagnostics;

namespace UserManagementAPI.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            var watch = Stopwatch.StartNew();
            await _next(context);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            // You can log the elapsed time here. For simplicity, we're writing to console.
            System.Console.WriteLine($"Request took: {elapsedMs} ms");
        }
    }
}
