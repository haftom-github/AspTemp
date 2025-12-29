using AspTemp.Features.Auth.Domain;
using AspTemp.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AspTemp.Features.Auth.Commands;

public interface IUserRepo
{
    Task AddAsync(User user, CancellationToken ct);
    Task<bool> UsernameExistsAsync(string username, CancellationToken ct);
    Task<bool> EmailExistsAsync(string email, CancellationToken ct);
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<User?> GetByAuthProviderAsync(Guid providerId, string providerUserId, CancellationToken ct);
    Task<bool> AuthIdentityExistsAsync(Guid providerId, string providerUserId, CancellationToken ct);
}

public class UserRepo(AppDbContext dbContext)
    : IUserRepo
{
    public async Task AddAsync(User user, CancellationToken ct)
    {
        await dbContext.Users.AddAsync(user, ct);
    }

    public Task<bool> UsernameExistsAsync(string username, CancellationToken ct)
    {
        return dbContext.Set<AuthIdentity>()
            .AnyAsync(ai => ai.ProviderUserId == username, cancellationToken: ct);
    }

    public Task<bool> EmailExistsAsync(string email, CancellationToken ct)
    {
        return dbContext.Users.AnyAsync(u => u.Email == email, cancellationToken: ct);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct)
    {
        return await dbContext.Users
            .Include(u => u.AuthIdentities)
            .ThenInclude(ai => ai.AuthProvider)
            .FirstOrDefaultAsync(u => u.AuthIdentities.Any(ai => ai.ProviderUserId == username), cancellationToken: ct);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return dbContext.Users.Include(u => u.AuthIdentities)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken: ct);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return dbContext.Users.Include(u => u.AuthIdentities)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken: ct);
    }

    public Task<User?> GetByAuthProviderAsync(Guid providerId, string providerUserId, CancellationToken ct)
    {
        return dbContext.Users
            .Include(u => u.AuthIdentities)
            .ThenInclude(ai => ai.AuthProvider)
            .FirstOrDefaultAsync(u => u.AuthIdentities.Any(ai => ai.AuthProviderId == providerId && ai.ProviderUserId == providerUserId), ct);
    }

    public Task<bool> AuthIdentityExistsAsync(Guid providerId, string providerUserId, CancellationToken ct)
    {
        return dbContext.Users
            .AnyAsync(u => u.AuthIdentities
                .Any(ai => ai.AuthProviderId == providerId && ai.ProviderUserId == providerUserId), ct);
    }
}