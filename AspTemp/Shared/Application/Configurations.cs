using System.Reflection;
using AspTemp.Shared.Application.Contracts.Cqrs;
using FluentValidation;

namespace AspTemp.Shared.Application;

public static class Configurations
{
    public static void ConfigureApplication(this IServiceCollection services, Assembly assembly)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddValidatorsFromAssembly(assembly, lifetime: ServiceLifetime.Scoped);
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            // config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
    }
}