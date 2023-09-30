using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Specifications
{
    public class ProductWithBrandAndTypeSpecification : BaseSpecification<Product>
    {
        public ProductWithBrandAndTypeSpecification(ProductSpecificationParams psp) 
            :base(
                x => 
                (string.IsNullOrEmpty(psp.Search) || x.Name.ToLower().Contains(psp.Search)) &&
                (!psp.BrandId.HasValue || x.ProductBrandId == psp.BrandId) &&
                (!psp.TypeId.HasValue || x.ProductTypeId == psp.TypeId))
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            ApplyPaging(psp.PageSize * (psp.PageIndex - 1), psp.PageSize);

            if (!string.IsNullOrEmpty(psp.Sort)) {
                switch (psp.Sort) {
                    case ProductSortingType.ProductSortingByNameAsc:
                        AddOrderBy(p => p.Name);
                        break;
                    case ProductSortingType.ProductSortingByNameDesc:
                        AddOrderByDescending(p => p.Name);
                        break;
                    case ProductSortingType.ProductSortingByPriceAsc:
                        AddOrderBy(p => p.Price);
                        break;
                    case ProductSortingType.ProductSortingByPriceDesc:
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
        }

        public ProductWithBrandAndTypeSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
            
        }
    }
}