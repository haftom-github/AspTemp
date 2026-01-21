using AspTemp.Shared.Infrastructure.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace AspTemp.Shared.Infrastructure;

public static class Configurations
{
    public static void ConfigureInfrastructure(this IServiceCollection services)
    {
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