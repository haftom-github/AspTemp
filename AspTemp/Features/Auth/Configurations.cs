using System.Text;
using AspTemp.Features.Auth.AuthProviders.Domain;
using AspTemp.Features.Auth.Users.Commands;
using AspTemp.Features.Auth.Users.Domain;
using AspTemp.Features.Auth.Users.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace AspTemp.Features.Auth;

public static class Configurations
{
    public static void ConfigureAuth(this IServiceCollection services, IConfiguration config)
    {
        var jwt = config.GetSection("Jwt");
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IUserRepo, UserRepo>();
        services.AddScoped<IAuthProviderRepo, AuthProviderRepo>();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt["Issuer"],
                    ValidAudience = jwt["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!))
                };
                
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies[jwt["AccessTokenSessionKey"]!];
                        return Task.CompletedTask;
                    }
                };
            });
        
        services.AddAuthorization();
    }
}