using AspTemp.Features.Auth.Commands;
using AspTemp.Features.Auth.Services;
using Microsoft.AspNetCore.Identity;

namespace AspTemp.Features.Auth;

public static class Configurations
{
    public static void ConfigureAuth(this IServiceCollection services)
    {
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IUserRepo, UserRepo>();
    }
}