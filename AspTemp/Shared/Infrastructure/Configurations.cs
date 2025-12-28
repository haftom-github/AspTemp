using Microsoft.EntityFrameworkCore;

namespace AspTemp.Shared.Infrastructure;

public static class Configurations
{
    public static void ConfigureInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=app.db"));
    }
}