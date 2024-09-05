using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Rat_Server.Custom_Middleware;
using Microsoft.IdentityModel.Tokens;
using Rat_Server.Model.Context;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Jwt configuration
var jwtIssuer = builder.Configuration["JWT_ISSUER"];
var jwtKey = builder.Configuration["JWT_KEY"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
 .AddJwtBearer(options =>
 {
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = jwtIssuer,
         ValidAudience = jwtIssuer,
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
     };
 });

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Perform a Dependency Injection for the MySQL Database Context
builder.Services.AddDbContext<RatDbContext>(options =>
    options.UseMySQL($"server={builder.Configuration["DATABASE_IP"]};" +
                     $"database={builder.Configuration["DATABASE_NAME"]};" +
                     $"user={builder.Configuration["DATABASE_USER"]};" +
                     $"password={builder.Configuration["DATABASE_PASSWORD"]}"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireClaim("Admin", "true"));
    options.AddPolicy("DeviceAuthenticated", policy => policy.RequireClaim("DeviceAuthenticated", "true"));
});

builder.Services.AddTransient<UpdateDeviceLastActive>();

var app = builder.Build();

app.UseFactoryActivatedMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
