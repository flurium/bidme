using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.Helpers
{
    [AttributeUsage(AttributeTargets.All)]
    public class NotBannedAs : Attribute, IAuthorizationFilter
    {
        private readonly string role;

        public NotBannedAs(string role)
        {
            this.role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User.IsInRole(role)) context.Result = new ViewResult { ViewName = "Banned" };
        }
    }
}