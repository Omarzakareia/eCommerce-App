using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories;
public interface IGenericRepository<T> where T : BaseEntity
{
    // Get All
    #region Without Spec
    Task<IReadOnlyList<T>> GetAllAsync();

    // Get by Id 
    Task<T> GetByIdAsync(int id);
    #endregion

    #region With Spec
    Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Spec);
    Task<T> GetEntityWithSpecAsync(ISpecification<T> Spec);
    #endregion
    Task<int> GetCountWithSpecAsync(ISpecification<T> Spec);
    Task AddAsync(T entity);                       // Add a new entity
    void Delete(T item);
    void Update(T item);

}
