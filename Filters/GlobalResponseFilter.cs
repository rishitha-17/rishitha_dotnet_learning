
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
namespace policy_management.Filters;
public class GlobalResponseFilter : IAsyncResultFilter
{
	public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
	{
	    var result = context.Result as ObjectResult;
        if (result != null)
        {
            var response = new
            {
                success = result.StatusCode >= 200 && result.StatusCode < 300,
                data = result.Value,    
                statusCode = result.StatusCode,
                message = result.StatusCode >= 200 && result.StatusCode < 300 ? "Request processed successfully." : "An error occurred while processing the request.",
                traceId = context.HttpContext.TraceIdentifier
            };
            context.Result = new ObjectResult(response)
            {
                StatusCode = result.StatusCode
            };
            await next();
        }
        
	}
}