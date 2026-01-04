using Microsoft.AspNetCore.Authentication;
using Shop.Web.Services;
using System.Net.Http;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Microservices + HttpClientFactory
const string PRODUCT_API = "PRODUCT_API";
string PRODUCT_API_URI = builder.Configuration["MicroservicesAddresses:ProductAPI"] ?? throw new ArgumentNullException("Introduce a api uri!");


builder.Services.AddHttpClient(PRODUCT_API, httpClient =>
{
    httpClient.BaseAddress = new Uri(PRODUCT_API_URI);
    httpClient.DefaultRequestHeaders.Accept.Clear();
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
    httpClient.DefaultRequestHeaders.Add("Keep-Alive", "3600");
    httpClient.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-ProductAPI");
});

// Microservices + HttpClientFactory
const string CART_API = "CART_API";
string CART_API_URI = builder.Configuration["MicroservicesAddresses:CartAPI"] ?? throw new ArgumentNullException("Introduce a api uri!");


builder.Services.AddHttpClient(CART_API, httpClient =>
{
    httpClient.BaseAddress = new Uri(CART_API_URI);
    httpClient.DefaultRequestHeaders.Accept.Clear();
    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

// Services DI
builder.Services.AddScoped<IProductService, ProductsService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies", c=> {
        c.ExpireTimeSpan = TimeSpan.FromMinutes(10);
        c.Events = new Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationEvents
        {
            OnSigningIn = async context =>
            {
                // Here you can implement logic that runs when the user signs in
                await Task.CompletedTask;
            },
            OnSignedIn = async context =>
            {
                // Here you can implement logic that runs after the user has signed in
                await Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                context.HttpContext.Response.Redirect(builder.Configuration["MicroservicesAddresses:IdentityServer"] + "/Account/AccessDenied");
                return Task.CompletedTask;
            }
        };

    })
    .AddOpenIdConnect("oidc", options =>
    {
        options.Events.OnRemoteFailure = context =>
        {

            context.Response.Redirect("/");
            context.HandleResponse();
            return Task.FromResult(0);
        };

        options.Authority = builder.Configuration["MicroservicesAddresses:IdentityServer"];
        options.GetClaimsFromUserInfoEndpoint = true;
        options.ClientId = "shop";
        options.ClientSecret = builder.Configuration["Client:Secret"];
        options.ResponseType = "code";
        options.ClaimActions.MapJsonKey("role", "role", "role");
        options.ClaimActions.MapJsonKey("sub", "sub", "sub");
        //options.ClaimActions.MapJsonKey("user_id", "user_id", "user_id");
        options.TokenValidationParameters.NameClaimType = "name";
        options.TokenValidationParameters.RoleClaimType = "role";
        options.Scope.Add("shop");
        options.SaveTokens = true;
        options.RequireHttpsMetadata = false;
    });

builder.Services.AddAuthorization();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
