using AspTemp.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AspTemp.Features.Auth.AuthProviders.Domain;

public interface IAuthProviderRepo
{
    Task<AuthProvider?> GetByNameAsync(string name);
}

public class AuthProviderRepo(AppDbContext dbContext) : IAuthProviderRepo
{
    private readonly DbSet<AuthProvider> _authProviders = dbContext.Set<AuthProvider>();
    public Task<AuthProvider?> GetByNameAsync(string name)
    {
        return _authProviders.FirstOrDefaultAsync(ap => ap.Name == name);
    }
}