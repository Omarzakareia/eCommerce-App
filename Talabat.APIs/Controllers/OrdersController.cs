using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Security.Claims;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.Core;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Service;
using Order = Talabat.Core.Entities.Order_Aggregate.Order;

namespace Talabat.APIs.Controllers
{

    public class OrdersController : ApiBaseController
    {

        #region Constructor & Attributes & Fields
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrdersController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            this._mapper = mapper;
        }
        #endregion


        #region EndPoints


        #region Create Order
        //Create Order
        [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [HttpPost] // POST => BaseUrl/api/Orders
        [Authorize]
        // If the body of the request has object , it will do rabing from body
        public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var MappedAddress = _mapper.Map<AddressDto, Address>(orderDto.shipToAddress);
            var Order = await _orderService.CreateOrderAsync(BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, MappedAddress);
            if (Order is null) return BadRequest(new ApiResponse(400, "Therer is a Problem With Your Order"));
            return Ok(Order);
        }
        #endregion


        #region GetOrdersForUser
        [ProducesResponseType(typeof(IReadOnlyList<OrderToReturnDto>), StatusCodes.Status200OK)] // Documentation
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet] // GET => BaseUrl/api/Orders
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Orders = await _orderService.GetOrdersForSpecificationAsync(BuyerEmail);
            if (Orders is null) return NotFound(new ApiResponse(404, "There is no Orders For this User"));
            var MappedOrders = _mapper.Map<IReadOnlyList<Order> , IReadOnlyList<OrderToReturnDto>>(Orders);
            return Ok(MappedOrders);

        }
        #endregion


        #region GetOrderForUser
        [ProducesResponseType(typeof(OrderToReturnDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")] // GET => BaseUrl/api/Orders/Id   // As A Segment NOT A Query String
        [Authorize]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdForUser(int id)
        {
            var BuyerEmail = User.FindFirstValue(ClaimTypes.Email);
            var Order = await _orderService.GetOrderByIdForSpecificUserAsync(BuyerEmail, id);
            if (Order is null) return NotFound(new ApiResponse(404, $"There is no Order With Id = {id} For this User"));
            var MappedOrders = _mapper.Map<Order, OrderToReturnDto>(Order);

            return Ok(MappedOrders);
        }
        #endregion
        

        #region GetDeliveryMethods
        [HttpGet("DeliveryMethods")] // GET => BaseUrl/api/Orders/DeliveryMethods   // As A Segment NOT A Query String
        [Authorize]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            var DeliveryMethods = await _orderService.GetDeliveryMethodsAsync();
            return Ok(DeliveryMethods);
        }
        #endregion 


        #endregion


    }
}
