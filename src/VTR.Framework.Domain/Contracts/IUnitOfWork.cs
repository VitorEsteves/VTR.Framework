namespace VTR.Framework.Domain.Contracts;

public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync(CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task RollbackAsync(CancellationToken cancellationToken);

    IEntityRepository<TEntity> GetEntityRepository<TEntity>(CancellationToken cancellationToken) where TEntity : Entity;
}