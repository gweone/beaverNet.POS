using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace beaverNet.POS.WebApp.Services
{
    public class PoSAuthorizationHandler : IAuthorizationHandler
    {
        IConfiguration configuration;
        IHttpContextAccessor _httpContextAccessor;
        public PoSAuthorizationHandler(IConfiguration configuration, IHttpContextAccessor _httpContextAccessor)
        {
            this.configuration = configuration;
            this._httpContextAccessor = _httpContextAccessor;
        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            if (IsAuthorize(context))
            {
                context.Succeed(new PoSAuthorizeRequirement());
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }

        bool IsAuthorize(AuthorizationHandlerContext context)
        {
            var routedata = _httpContextAccessor.HttpContext.GetRouteData();
            if (!routedata.Values.ContainsKey("controller"))
                return true;
            string controller = routedata.Values["controller"].ToString();
            string action = _httpContextAccessor.HttpContext.GetRouteData().Values["action"].ToString();
            var claim = string.Format("{0}.{1}", controller, action).ToLowerInvariant();
            var authorize = context.User.HasClaim(x => x.Type == ClaimTypes.UserData && x.Value == claim);
            return authorize;
        }
    }
}
