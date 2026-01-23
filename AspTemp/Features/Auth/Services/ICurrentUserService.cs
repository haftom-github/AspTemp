using System.Security.Claims;

namespace AspTemp.Features.Auth.Services;

public interface ICurrentUserService
{
    Guid? GetUserId();
}

public class CurrentUserService(
    IHttpContextAccessor httpContextAccessor
) : ICurrentUserService
{
    public Guid? GetUserId()
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

        return Guid.TryParse(idClaim, out var id) ? id : null;
    }
}