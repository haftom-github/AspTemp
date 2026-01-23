using System.Reflection;
using AspTemp.Shared.Cqrs;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AspTemp.Shared;

public static class Configurations
{
    public static void ConfigureShared(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddValidatorsFromAssembly(assembly, lifetime: ServiceLifetime.Scoped);
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            // config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        
        services.AddHttpContextAccessor();
        services.AddScoped<AuditSaveChangesInterceptor>();

        services.AddDbContext<AppDbContext>((provider, options) =>
        {
            options.UseSqlite("Data Source=app.db");
            var interceptor = provider.GetRequiredService<AuditSaveChangesInterceptor>();
            options.AddInterceptors(interceptor);
        });
    }
}