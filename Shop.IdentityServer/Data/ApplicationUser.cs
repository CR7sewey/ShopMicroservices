using Microsoft.AspNetCore.Identity;

namespace Shop.IdentityServer.Context
{
    public class ApplicationUser : IdentityUser
    {

        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty ;

    }
}
