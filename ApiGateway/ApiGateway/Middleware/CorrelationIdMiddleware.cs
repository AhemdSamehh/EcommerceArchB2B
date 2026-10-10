namespace ApiGateway.Middleware
{
    public class CorrelationIdMiddleware
    {
        private const string CorrelationIdHeader = "x-correlation-id";
        private readonly RequestDelegate _next;
        private readonly ILogger<CorrelationIdMiddleware> _logger;

        public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(CorrelationIdHeader, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers[CorrelationIdHeader] = correlationId;
                _logger.LogInformation("Generated new Correlation ID: {CorrelationId}", correlationId);
            }
            else
            {
                _logger.LogInformation("Received Correlation ID from request: {CorrelationId}", correlationId);
            }
            context.Response.OnStarting(() =>
            {
                context.Response.Headers[CorrelationIdHeader] = correlationId;
                return Task.CompletedTask;
            });
            await _next(context);
        }
    }
}
