using System.Security.Claims;
using AspTemp.Features.Auth.Commands;
using AspTemp.Features.Auth.Domain;

namespace AspTemp.Features.Auth.Services;

public interface ICurrentUserService
{
    Task<User?> GetAsync(CancellationToken ct = default);
}

public class CurrentUserService(
    IUserRepo userRepo,
    IHttpContextAccessor httpContextAccessor
) : ICurrentUserService
{
    public async Task<User?> GetAsync(CancellationToken ct = default)
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null)
            return null;

        var contextUser = httpContext.User;
        if (contextUser.Identity?.IsAuthenticated != true)
            return null;

        var idClaim = contextUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(idClaim))
            return null;

        if (!Guid.TryParse(idClaim, out var guid)) return null;
        var user = await userRepo.GetByIdAsync(guid, ct);
        return user;
    }
}