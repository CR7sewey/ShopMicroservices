

using Microsoft.EntityFrameworkCore;
using Shop.CartAPI.Context;
using Shop.CartAPI.Models.DTOs;
using Shop.CartAPI.Repositories;
using Shop.CartAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("Insert a connection string please...");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

// Automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Repositories DI
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
// Services DI
builder.Services.AddScoped<CartService, CartService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
           options.SwaggerEndpoint("/openapi/v1.json", "Catalogo API - Cart"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
