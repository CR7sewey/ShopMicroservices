using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.IdentityServer.Configuration;
using Shop.IdentityServer.Context;
using Shop.IdentityServer.IdentityServer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string"
        + "'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


// Identity - binds the identity system to the enity framework context

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(//options => // perfis do identity
//{
    //options.Password.RequireDigit = true;
    //options.Password.RequireLowercase = true;
    //options.Password.RequireUppercase = true;
   // options.Password.RequireNonAlphanumeric = false;
   // options.Password.RequiredLength = 6;
   // options.User.RequireUniqueEmail = true;
//}
)
   .AddEntityFrameworkStores<ApplicationDbContext>() // usar o EF para armazenar os dados do identity
   .AddDefaultTokenProviders(); // provedores de token padrao (recuperacao de senha, confirmacao de email, etc)


////////
builder.Services.AddControllersWithViews();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// IDENTITY SERVER
var builderIdentityServer = builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
})
    .AddInMemoryIdentityResources(IdentityConfiguration.IdentityResources)
    .AddInMemoryApiScopes(IdentityConfiguration.ApiScopes)
    .AddInMemoryClients(IdentityConfiguration.Clients)
    .AddAspNetIdentity<ApplicationUser>()
    ;

builderIdentityServer.AddDeveloperSigningCredential();


builder.Services.AddScoped<IDatabaseSeedInitializar, DatabaseIdentityServerInitializer>();


builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.UseIdentityServer(); // activate middleware

app.UseAuthorization();

await SeedDatabaseIdentityServer(app);

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

async Task SeedDatabaseIdentityServer(IApplicationBuilder app)
{
   using (var service = app.ApplicationServices.CreateScope())
    {
        var initRolesUsers = service.ServiceProvider.GetService<IDatabaseSeedInitializar>();
        if (initRolesUsers != null)
        {
           await initRolesUsers.InitializeSeedRoles();
           await initRolesUsers.InitializeSeedUsers();
        }
    }
}