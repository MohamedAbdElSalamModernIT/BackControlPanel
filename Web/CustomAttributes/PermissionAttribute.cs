using System;
using System.Linq;
using Common;
using Common.Attributes;
using Common.Exceptions;
using Common.Extensions;
using Domain.Enums.Roles;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Web.CustomAttributes
{
   public class PermissionAttribute : Attribute, IActionFilter
    {
        public PermissionKeys Permission { get; set; }

        public PermissionAttribute(PermissionKeys permission) 
        {
            
            Permission = permission;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var name = this.Permission.GetAttribute<DescribePermissionAttribute>().Key;

            var permissions = context.HttpContext.User.Claims.SingleOrDefault(c => c.Type == "permissions").Value;

            if (!permissions.Contains(name))
                throw new ApiException(ApiExceptionType.Forbidden);
 
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
           
        }
    }
}
