using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext) // Ask CLR for creating object from dbcontext implicitly
        {
            _dbContext = dbContext;
        }
        #region without spec
        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await _dbContext.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id)
            => await _dbContext.Set<T>().FindAsync(id);
        #endregion



        private IQueryable<T> ApplySpec(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), spec);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> Spec)
        {
            //return await SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>(), Spec).ToListAsync();
            return await ApplySpec(Spec).ToListAsync();
        }

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> Spec)
        {
            return await ApplySpec(Spec).FirstOrDefaultAsync();

        }

        public async Task<int> GetCountWithSpecAsync(ISpecification<T> Spec)
        {
            return  await ApplySpec(Spec).CountAsync();
        }

        public async Task Add(T item)
        {
            await _dbContext.AddAsync(item);
        }

        public void Delete(T item) => _dbContext.Set<T>().Remove(item);
        public void Update(T item) => _dbContext.Set<T>().Update( item);

    }
}
