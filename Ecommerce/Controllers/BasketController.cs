using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Entities;
using Core.IRepositories;
using Ecommerce.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        [HttpGet("getbasketbyid/{id}")]
        public async Task<IActionResult> GetBasketById([FromRoute] string id)
        {
            var basket = await _basketRepository.GetCustomerBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost("updatebasket")]
        public async Task<IActionResult> UpdateBasket(CustomerBasketDto basket)
        {
            var customerBasket = _mapper.Map<CustomerBasket>(basket);
            var updated = await _basketRepository.UpdateCustomerBasketAsync(customerBasket);
            return Ok(updated);
        }

        [HttpDelete("deletebasket/{id}")]
        public async Task DeleteBasket([FromRoute] string id)
        {
            await _basketRepository.DeleteCustomerBasketAsync(id);
        }
    }
}