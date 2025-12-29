using System.Reflection;
using AspTemp.Features.Auth.AuthProviders.Domain;
using AspTemp.Features.Auth.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace AspTemp.Shared.Infrastructure;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{ 
    public DbSet<User> Users => Set<User>();
    public DbSet<AuthProvider> AuthProviders => Set<AuthProvider>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}