namespace ECommerce.API.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;

    /// <summary>
    /// Middleware that logs HTTP request and response details, including headers, status codes, and execution time.
    /// </summary>
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        /// <param name="logger">The logger instance.</param>
        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware to log request and response details.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            // Log request details
            _logger.LogInformation("Request: {method} {url} Headers: {headers}",
                context.Request.Method,
                context.Request.Path,
                context.Request.Headers);

            // Copy original response body to restore later
            var originalBodyStream = context.Response.Body;
            using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            stopwatch.Stop();

            // Read response body
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseText = new StreamReader(context.Response.Body).ReadToEnd();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            // Log response details
            _logger.LogInformation("Response: {statusCode} Headers: {headers} Body: {body} ExecutionTime: {elapsed}ms",
                context.Response.StatusCode,
                context.Response.Headers,
                responseText,
                stopwatch.ElapsedMilliseconds);

            // Restore original response body
            await responseBody.CopyToAsync(originalBodyStream);
        }
    }
}
