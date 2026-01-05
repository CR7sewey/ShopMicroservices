using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Shop.DiscountAPI.Context;
using Shop.DiscountAPI.Models;
using Shop.DiscountAPI.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// DB Injection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Insert a connection string please...");
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(connectionString)
        .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning))
);

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ICouponRepository, CouponRepository>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

// CORS
var frontendURI = "http://localhost:5011";
builder.Services.AddCors(cors =>
{
    cors.AddPolicy(name: "WEB", policy =>
    {
        policy.WithOrigins(frontendURI);
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

builder.Services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
{
    options.Authority = builder.Configuration["Shop.IdentityServer.ApplicationUrl"];
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = false, // valida o publico do token

    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "shop");
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
           options.SwaggerEndpoint("/openapi/v1.json", "Discount API"));

}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("WEB");
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
