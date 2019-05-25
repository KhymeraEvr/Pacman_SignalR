using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using PacmanWebb.Areas.Identity.Pages.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PacmanWebb.Areas.Identity
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<PacmanUser, IdentityRole>
    {
        public AppClaimsPrincipalFactory(
            UserManager<PacmanUser> userManager
            , RoleManager<IdentityRole> roleManager
            , IOptions<IdentityOptions> optionsAccessor)
        : base(userManager, roleManager, optionsAccessor)
        { }

        public async override Task<ClaimsPrincipal> CreateAsync(PacmanUser user)
        {
            var principal = await base.CreateAsync(user);

            if (!string.IsNullOrWhiteSpace(user.PlayerName))
            {
                ((ClaimsIdentity)principal.Identity).AddClaims(new[] {
        new Claim(ClaimTypes.GivenName, user.PlayerName)
    });
            }         
            return principal;
        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(PacmanUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("GivenName", user.PlayerName ?? ""));
            return identity;
        }
    }
}
