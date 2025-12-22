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
});

// Services DI
builder.Services.AddScoped<IProductService, ProductsService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();


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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
