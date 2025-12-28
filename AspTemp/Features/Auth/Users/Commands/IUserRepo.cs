using AspTemp.Features.Auth.Users.Domain;
using AspTemp.Shared.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AspTemp.Features.Auth.Users.Commands;

public interface IUserRepo
{
    Task AddAsync(User user, CancellationToken ct);
    Task<bool> UsernameExistsAsync(string username, CancellationToken ct);
    Task<bool> EmailExistsAsync(string email, CancellationToken ct);
    Task<User?> GetByUsernameAsync(string username, CancellationToken ct);
    Task<User?> GetByEmailAsync(string email, CancellationToken ct);
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
}

public class UserRepo(AppDbContext dbContext)
    : IUserRepo
{
    private readonly DbSet<User> _users = dbContext.Set<User>();
    
    public async Task AddAsync(User user, CancellationToken ct)
    {
        await _users.AddAsync(user, ct);
    }

    public Task<bool> UsernameExistsAsync(string username, CancellationToken ct)
    {
        return dbContext.Set<AuthIdentity>()
            .AnyAsync(ai => ai.ProviderUserId == username, cancellationToken: ct);
    }

    public Task<bool> EmailExistsAsync(string email, CancellationToken ct)
    {
        return _users.AnyAsync(u => u.Email == email, cancellationToken: ct);
    }

    public Task<User?> GetByUsernameAsync(string username, CancellationToken ct)
    {
        return _users.Include(u => u.AuthIdentities)
            .FirstOrDefaultAsync(u => u.AuthIdentities.Any(ai => ai.ProviderUserId == username), cancellationToken: ct);
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return _users.Include(u => u.AuthIdentities)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken: ct);
    }

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return _users.Include(u => u.AuthIdentities)
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken: ct);
    }
}