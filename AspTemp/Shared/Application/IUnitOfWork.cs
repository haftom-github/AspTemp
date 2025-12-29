using AspTemp.Shared.Infrastructure;

namespace AspTemp.Shared.Application;

public interface IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken);
}

public class UnitOfWork(AppDbContext dbContext): IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}