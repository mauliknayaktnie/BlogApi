using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlogBackend.Models;  // Add this namespace for your DbContext

var builder = WebApplication.CreateBuilder(args);

//  Database Connection String
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//  Register BlogDbContext
builder.Services.AddDbContext<BlogDbContext>(options =>
    options.UseSqlServer(connectionString)  // UseSqlServer for SQL Server, UseSqlite for SQLite
);

//  Ensure JWT Key is present
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 16)
{
    throw new Exception("JWT Key is missing or too short! It must be at least 16 characters.");
}

//  Add Authentication Services
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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

//  Add Authorization Services
builder.Services.AddAuthorization();

//  Add Controllers
builder.Services.AddControllers();

//  Register Your Controllers
builder.Services.AddScoped<AuthController>();

var app = builder.Build();

//  Middleware Configuration
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
