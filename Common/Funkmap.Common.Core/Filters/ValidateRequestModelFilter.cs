using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Funkmap.Common.Core.Filters
{
    public class ValidateRequestModelFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var badRequest = new BadRequestResult();
            if (context.ModelState.IsValid == false)
            {
                context.HttpContext.Response.StatusCode = badRequest.StatusCode;
                return;
            }

            if (context.ActionArguments.Values.Contains(null) && context.ActionArguments.Values.Contains(""))
            {
                context.HttpContext.Response.StatusCode = badRequest.StatusCode;
                return;
            }

            await next();
        }
    }
}
