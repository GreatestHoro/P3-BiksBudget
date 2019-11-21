using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FrontEnd2.Data
{
    class AuthenticationFunctionallity
    {

        public class ServerAuthenticationStateProvider : AuthenticationStateProvider
        {
            public override async Task<AuthenticationState> GetAuthenticationStateAsync()
            {
                var claim = new Claim(ClaimTypes.Name, "Fake user");
                var identity = new ClaimsIdentity(new[] { claim }, "serverauth");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
        }
    }
}
