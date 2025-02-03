using System.Linq.Expressions;

namespace VTR.Framework.Domain.Contracts;

public interface IEntityRepository<TEntity> where TEntity : Entity
{
    OperationResult Update(TEntity entity, string successMessage);

    OperationResult Insert(TEntity entity, string successMessage);

    void Delete(TEntity entity);

    Task<TEntity?> GetByIdAsync(Guid? id);

    Task<TEntity?> GetByAsync(Expression<Func<TEntity, bool>> predicate);

    Task<bool> ExistAsync(Expression<Func<TEntity, bool>> predicate);

    Task<List<TEntity>> ListAsync();

    Task<List<TEntity>> ListByIdsAsync(IEnumerable<Guid> id);

    Task<List<TEntity>> ListByAsync(Expression<Func<TEntity, bool>> predicate);

    Task<List<TEntity>> ListByAsync<TProperty>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TProperty>> include);

    Task<TEntity> GetMaxAsync(Expression<Func<TEntity, TEntity>> selector);

    Task<TEntity> GetMaxByAsync(Expression<Func<TEntity, TEntity>> selector, Expression<Func<TEntity, bool>> predicate);
}