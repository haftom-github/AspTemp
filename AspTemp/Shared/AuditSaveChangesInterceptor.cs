using AspTemp.Features.Auth.Services;
using AspTemp.Shared.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AspTemp.Shared;

public class AuditSaveChangesInterceptor(ICurrentUserService currentUserService) 
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var userId = currentUserService.GetUserId();
        UpdateEntities(eventData.Context, userId);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var userId = currentUserService.GetUserId();
        UpdateEntities(eventData.Context, userId);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
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
