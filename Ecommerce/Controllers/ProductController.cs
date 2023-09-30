using AutoMapper;
using Core.Entities;
using Core.IRepositories;
using Core.Specifications;
using Ecommerce.Dtos;
using Ecommerce.Helpers;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : BaseApiController
    {
        private readonly IProductRepository _productRepository;
        private readonly IGenericRepository<ProductBrand> _productBrandRepository;
        private readonly IGenericRepository<ProductType> _productTypeRepository;
        private readonly IGenericRepository<Product> _productRepositoryExtra;
        private readonly IMapper _mapper;
        public ProductController(
            IProductRepository productRepository,
            IGenericRepository<ProductBrand> productBrandRepository,
            IGenericRepository<ProductType> productTypeRepository,
            IGenericRepository<Product> productRepositoryExtra,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _productBrandRepository = productBrandRepository;
            _productTypeRepository = productTypeRepository;
            _productRepositoryExtra = productRepositoryExtra;
            _mapper = mapper;
        }

        [HttpGet("getproducts")]
        public async Task<IActionResult> GetProducts([FromQuery]ProductSpecificationParams psp) {
            var spec = new ProductWithBrandAndTypeSpecification(psp);

            var countSpec = new ProductWithFiltersForCountSpecification(psp);

            var totalItems = await _productRepositoryExtra.CountAsync(countSpec);

            var products = await _productRepositoryExtra.GetListEntityWithSpecAsync(spec); // anti pattern
            
            var data = _mapper.Map<IReadOnlyList<ProductDto>>(products);

            var result = new Pagination<ProductDto>(psp.PageSize, psp.PageIndex, totalItems, data);

            return Ok(result);
        }

        [HttpGet("getproductbyid/{id}")]
        public async Task<IActionResult> GetProduct([FromRoute]int id) {
            var spec = new ProductWithBrandAndTypeSpecification(id);
            var product = await _productRepositoryExtra.GetEntityWithSpecAsync(spec);
            if (product == null) {
                return NotFound();
            }
            return Ok(_mapper.Map<ProductDto>(product));
        }

        [HttpGet("brands")]
        public async Task<IActionResult> GetProducBrands()
        {
            var brands = await _productBrandRepository.GetAllAsync();
            if (!brands.Any()) {
                return NotFound();
            }
            return Ok(brands);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetProductTypes()
        {
            var types = await _productTypeRepository.GetAllAsync();
            if (!types.Any()) {
                return NotFound();
            }
            return Ok(types);
        }
    }
}