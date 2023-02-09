using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Helpers
{
    public class NotBannedAs : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (Roles != null && context.HttpContext.User.IsInRole(Roles))
            {
                context.Result = new ViewResult { ViewName = "Banned" };
            }
        }
    }
}