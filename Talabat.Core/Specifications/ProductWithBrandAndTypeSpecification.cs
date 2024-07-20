using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product>
    {
        // CTOR is used for Gett All Products
        public ProductWithBrandAndTypeSpecification(ProductSpecParams specParams)
            :base(p => 
            (string.IsNullOrEmpty(specParams.Search)|| p.Name.ToLower().Contains(specParams.Search))
            &&
            (!specParams.BrandId.HasValue || p.ProductBrandId == specParams.BrandId)
            && 
            (!specParams.TypeId.HasValue || p.ProductTypeId == specParams.TypeId)
            )
        {
            Includes.Add(P=>P.ProductType); 
            Includes.Add(P => P.ProductBrand);
            if(!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDesc(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }

            ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

        }
        public ProductWithBrandAndTypeSpecification(int id ) :base(P=>P.Id == id)
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
        }
    }
}
