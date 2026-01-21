using AspTemp.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Security.Claims;

namespace AspTemp.Shared.Infrastructure.Interceptors;

public class AuditSaveChangesInterceptor(IHttpContextAccessor httpContextAccessor) 
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        // Resolve current user id synchronously for the sync path (avoid changing EF sync flow)
        var userId = GetUserIdFromHttpContext();
        UpdateEntities(eventData.Context, userId);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var userId = GetUserIdFromHttpContext();
        UpdateEntities(eventData.Context, userId);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private Guid? GetUserIdFromHttpContext()
    {
        var httpContext = httpContextAccessor.HttpContext;
        if (httpContext == null) return null;

        var user = httpContext.User;
        if (user.Identity?.IsAuthenticated != true) return null;

        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(idClaim)) return null;
        return Guid.TryParse(idClaim, out var guid) ? guid : null;
    }

    private static void UpdateEntities(DbContext? context, Guid? userId)
    {
        if (context is null) return;

        var entries = context.ChangeTracker.Entries()
            .Where(e => e.Entity is IAuditable)
            .ToList();

        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            var auditable = (IAuditable)entry.Entity;
            switch (entry.State)
            {
                case EntityState.Added:
                    if (auditable.CreatedDate == default)
                        auditable.CreatedDate = now;
                    if (auditable.CreatedBy is null && userId is not null)
                        auditable.CreatedBy = userId;
                    auditable.RecordStatus = RecordStatus.Active;
                    break;
                case EntityState.Modified:
                    auditable.UpdatedDate = now;
                    auditable.UpdatedBy = userId;
                    break;
                case EntityState.Deleted:
                    auditable.RecordStatus = RecordStatus.Deleted;
                    auditable.UpdatedDate = now;
                    auditable.UpdatedBy = userId;
                    entry.State = EntityState.Modified;
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
