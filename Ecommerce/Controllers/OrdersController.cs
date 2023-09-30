using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Extensions;
using Core.IServices;
using Ecommerce.Dtos;
using Ecommerce.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }


        [HttpPost("createorder")]
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var email = HttpContext.User?.RetriveEmailFromPrincipal();

            var address = _mapper.Map<AddressDto, Address>(orderDto.ShipToAddress);

            var order = await _orderService.CreateOrderAsync(email, orderDto.DeliveryMethodId, orderDto.BasketId, address);

            if (order == null) return BadRequest(new ApiResponseError(400, "Proplem on creating order!"));
            
            return Ok(order);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var email = HttpContext.User?.RetriveEmailFromPrincipal();
            var orders = await _orderService.GetOrdersForUserAsync(email);
            return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDto>>(orders));
        }

        [HttpGet("getorderbyid/{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser([FromRoute] int id)
        {
            var email = HttpContext.User?.RetriveEmailFromPrincipal();
            var order = await _orderService.GetOrderByIdAsync(id, email);
            if (order == null) return NotFound(new ApiResponseError(404));
            return Ok(_mapper.Map<OrderToReturnDto>(order));
        }

        [HttpGet("deliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }
    }
}
