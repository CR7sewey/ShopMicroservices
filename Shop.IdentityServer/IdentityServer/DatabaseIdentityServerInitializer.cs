using Duende.IdentityModel;
using Microsoft.AspNetCore.Identity;
using Shop.IdentityServer.Configuration;
using Shop.IdentityServer.Context;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Shop.IdentityServer.IdentityServer
{
    public class DatabaseIdentityServerInitializer : IDatabaseSeedInitializar
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public DatabaseIdentityServerInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> identityRole)
        {
            this.userManager = userManager;
            this.roleManager = identityRole;
        }

        public async Task InitializeSeedRoles()
        {
            var clientExists = await roleManager.RoleExistsAsync(IdentityConfiguration.CLIENT);
            var adminExists = await roleManager.RoleExistsAsync(IdentityConfiguration.ADMIN);

            if (clientExists == false)
            {
                IdentityRole identityRole = new IdentityRole();
                identityRole.Name = IdentityConfiguration.CLIENT;
                identityRole.NormalizedName = IdentityConfiguration.CLIENT.ToUpper();
                await roleManager.CreateAsync(identityRole);

            }
            if (adminExists == false)
            {
                IdentityRole identityRole = new IdentityRole();
                identityRole.Name = IdentityConfiguration.ADMIN;
                identityRole.NormalizedName = IdentityConfiguration.ADMIN.ToUpper();
                await roleManager.CreateAsync(identityRole);

            }
        }

        public async Task InitializeSeedUsers() // random users
        {
            if (userManager.FindByEmailAsync("admin1@gmail.com").Result == null)
            {
                ApplicationUser applicationUser = new ApplicationUser()
                {
                    UserName = "admin1",
                    NormalizedUserName = "ADMIN1",
                    Email = "admin1@gmail.com",
                    NormalizedEmail = "admin1@gmail.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    PhoneNumber = "919191919",
                    FirstName = "Admin1", LastName = "Admin1",
                    SecurityStamp = Guid.NewGuid().ToString(),
                };

                var resultAdmin =  await userManager.CreateAsync(applicationUser, "Secret123#");
                if (resultAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(applicationUser, IdentityConfiguration.ADMIN);// .Wait();

                    var adminClaims = userManager.AddClaimsAsync(applicationUser,
                        new Claim[] {
                            new Claim(JwtClaimTypes.Name, $"{applicationUser.FirstName} {applicationUser.LastName}"),
                            new Claim(JwtClaimTypes.GivenName, applicationUser.FirstName),
                            new Claim(JwtClaimTypes.FamilyName, applicationUser.LastName),
                            new Claim(JwtClaimTypes.Role, IdentityConfiguration.ADMIN)
                        }).Result;
                }

            }
        }
    }
}
