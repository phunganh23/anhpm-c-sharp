using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace danh_gia_csharp.middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthorizationAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"];
            if (user == null)
            {
                context.HttpContext.Response.StatusCode = 401; 
                context.Result = new Microsoft.AspNetCore.Mvc.JsonResult(new { message = "Unauthorized" });
            }
        }
    }
}
