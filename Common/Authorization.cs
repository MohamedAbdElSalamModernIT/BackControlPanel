using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
   public class AuthorizationAttribute: AuthorizeAttribute
    {

        public AuthorizationAttribute()
        {
            this.AuthenticationSchemes = "Bearer";
        }
    }
}
