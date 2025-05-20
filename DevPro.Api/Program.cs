using DevPro.Application;
using DevPro.Application.Interface.Auth;
using DevPro.Database;
using DevPro.Domain;
using DevPro.Domain.Entities.Identity;
using DevPro.Services;
using DevPro.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace DevPro.Api;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        // Swagger
        builder.Services.AddSwaggerGen(option =>
        {
            option.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DevPro API",
                Version = "v1",
                Description = "API for DevPro Application",
                Contact = new OpenApiContact
                {
                    Name = "Diranujan",
                    Email = "diranujan2000@gmail.com"
                }
            });

            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });

            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        builder.Services.AddDbContext<DevProLocalDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Add Identity
        // Add this BEFORE registering your own services
        builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<DevProLocalDbContext>()
            .AddDefaultTokenProviders();

        // Configure Identity options (optional)
        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Password settings
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 1;

            // User settings
            options.User.RequireUniqueEmail = true;
        });



        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme =
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var signingKey = builder.Configuration["JWT:SigningKey"];
            if (string.IsNullOrEmpty(signingKey))
            {
                throw new InvalidOperationException("JWT SigningKey is not configured.");
            }
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = builder.Configuration["JWT:Issuer"],
                ValidateAudience = false,
                ValidAudience = builder.Configuration["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    System.Text.Encoding.UTF8.GetBytes(signingKey)),
                ValidateLifetime = true,
                RequireExpirationTime = true,
                ClockSkew = TimeSpan.Zero
            };
            options.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {

                        context.Response.Headers.Append("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                }
            };
        });


        builder.Services.AddAuthorization();
        //builder.Services.AddDbContext<DevProLocalDbContext>(x => x.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


        builder.Services.AddScoped<IProductService,ProductService>();

        builder.Services.AddScoped<IAccountRegisterService, AccountRegisterService>();

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        });

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
