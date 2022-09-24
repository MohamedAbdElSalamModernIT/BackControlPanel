using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Web.CustomAttributes {
    public class SessionAttribute : Attribute, IAsyncActionFilter {
        
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) {
            var sessionId = context.HttpContext.Request.Headers.ContainsKey("sessionId");
            if (!sessionId) {
                context.HttpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.HttpContext.Response.Clear();
                context.HttpContext.Response.ContentType = "application/json";
                return;
                
            }

            await next();

        }
    }
}