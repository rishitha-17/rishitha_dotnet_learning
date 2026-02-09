using Microsoft.AspNetCore.Mvc.Filters;

namespace policy_management.Filters
{
    public class ResponseTimeFilter : ActionFilterAttribute
    {
        private readonly ILogger<ResponseTimeFilter> _logger;

        public ResponseTimeFilter(ILogger<ResponseTimeFilter> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Store request start time in ticks for manual calculation
            var startTime = DateTime.UtcNow;
            context.HttpContext.Items["RequestStartTicks"] = startTime.Ticks;
            
            _logger.LogInformation("Request started at {RequestTime} for {Method} {Path}", 
                startTime, 
                context.HttpContext.Request.Method, 
                context.HttpContext.Request.Path);

            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            // Manual calculation of response time
            if (context.HttpContext.Items["RequestStartTicks"] is long startTicks)
            {
                var endTime = DateTime.UtcNow;
                var endTicks = endTime.Ticks;
                
                // Manual calculation: ticks difference to milliseconds
                var ticksDifference = endTicks - startTicks;
                var responseTimeMs = ticksDifference / TimeSpan.TicksPerMillisecond;

                _logger.LogInformation("Request completed at {ResponseTime} for {Method} {Path}. Manual calculated duration: {Duration}ms", 
                    endTime,
                    context.HttpContext.Request.Method, 
                    context.HttpContext.Request.Path,
                    responseTimeMs);

                // Add manually calculated response time to headers
                context.HttpContext.Response.Headers.Add("X-Response-Time", $"{responseTimeMs}ms");
            }
        }
    }
}

