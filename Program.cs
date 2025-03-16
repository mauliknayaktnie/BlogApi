using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlogBackend.Models;

var builder = WebApplication.CreateBuilder(args);

// Database Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register BlogDbContext
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(connectionString));

// Ensure JWT Key is Strong Enough
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)  // Require at least 32 chars
{
    throw new Exception("JWT Key is missing or too short! It must be at least 32 characters for security.");
}

// Add Authentication Services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.Zero // Ensures token expiration is accurate
        };
    });

// Add Authorization Services
builder.Services.AddAuthorization();

// Add Controllers
builder.Services.AddControllers();

// Add Swagger (Enable API Documentation)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger
if (app.Environment.IsDevelopment()) // Show Swagger in Development only
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware Configuration
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
