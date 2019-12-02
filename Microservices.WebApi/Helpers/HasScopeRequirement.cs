using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microservices.WebApi.Helpers
{
    public class HasScopeRequirement : AuthorizationHandler<HasScopeRequirement>, IAuthorizationRequirement
    {
        private string _scope;
        private string _issuer;
        public HasScopeRequirement(string scope, string issuer)
        {
            _scope = scope;
            _issuer = issuer;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, HasScopeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "scope" && c.Issuer == _issuer))
                return Task.CompletedTask;
            var scopes = context.User.FindFirst(c => c.Type == "scope" && c.Issuer == _issuer).Value.Split(" ");
            if (scopes.Any(s => s == _scope))
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
