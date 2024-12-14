using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository;
public static class SpecificationEvaluator<T> where T : BaseEntity
{
    public static IQueryable<T> GetQuery(IQueryable<T> StartQuery , ISpecification<T> Spec)
    {
        var Query = StartQuery; // _dbContext.Products
        // Criteria = p => p.Id == id
        if(Spec.Criteria != null)
        {
            Query = Query.Where(Spec.Criteria);
        }
        // _dbContext.Products.Where(p => p.Id == id)
        if(Spec.OrderBy != null)
            Query = Query.OrderBy(Spec.OrderBy);
        if (Spec.OrderByDescending != null)
            Query = Query.OrderByDescending(Spec.OrderByDescending);

        if (Spec.IsPaginationEnabled)
        {
            Query = Query.Skip(Spec.Skip).Take(Spec.Take);
        }

        Query = Spec.Includes.Aggregate(Query, (CurrentQuery, IncludeExp) => CurrentQuery.Include(IncludeExp));

        return Query;
    }
}
