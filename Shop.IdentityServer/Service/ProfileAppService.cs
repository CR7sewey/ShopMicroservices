using Duende.IdentityModel;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Shop.IdentityServer.Context;
using System.Security.Claims;

namespace Shop.IdentityServer.Service
{
    public class ProfileAppService : IProfileService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;

        public ProfileAppService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
        }

        // Adiciona as claims personalizadas ao token
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {

            ClaimsPrincipal subject = context.Subject;
            var user = await _userManager.FindByIdAsync(subject.GetSubjectId());
            if (user == null) {
                throw new ArgumentException("Invalid subject identifier");
            }
            // Criar ClaimsPrincipal a partir do usuário
            var claims = await _userClaimsPrincipalFactory.CreateAsync(user);
            var claimsList = claims.Claims.ToList();
            claimsList.Add(new Claim("user_id", user.Id));
            claimsList.Add(new Claim(JwtClaimTypes.Email, user.Email ?? ""));
            claimsList.Add(new Claim(JwtClaimTypes.GivenName, user.FirstName ?? ""));
            claimsList.Add(new Claim(JwtClaimTypes.FamilyName, user.LastName ?? ""));

            // se usermanager suporta roles
            if (_userManager.SupportsUserRole)
            {
                var roles = await _userManager.GetRolesAsync(user);
                foreach (var roleName in roles)
                {
                    claimsList.Add(new Claim(JwtClaimTypes.Role, roleName));
                    // se rolemanager suporta claims para roles
                    if (_roleManager.SupportsRoleClaims)
                    {
                        var role = await _roleManager.FindByNameAsync(roleName);
                        if (role != null)
                        {
                            var roleClaims = await _roleManager.GetClaimsAsync(role);
                            foreach (var roleClaim in roleClaims)
                            {
                                claimsList.Add(roleClaim);
                            }
                        }
                    }
                }
            }
            // retornar as claims no contexto
            context.AddRequestedClaims(claimsList);
        }

        // Verifica se o usuário está ativo
        public async Task IsActiveAsync(IsActiveContext context)
        {
            ClaimsPrincipal subject = context.Subject;
            var user = await _userManager.FindByIdAsync(subject.GetSubjectId());
            context.IsActive = user is not null;
        }
    }
}
