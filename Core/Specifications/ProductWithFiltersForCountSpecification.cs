using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        public ProductWithFiltersForCountSpecification(ProductSpecificationParams psp)
            :base(
                x => 
                (string.IsNullOrEmpty(psp.Search) || x.Name.ToLower().Contains(psp.Search)) &&
                (!psp.BrandId.HasValue || psp.BrandId == x.ProductBrandId) &&
                (!psp.TypeId.HasValue || psp.TypeId == x.ProductTypeId))
        {
            
        }
    }
}