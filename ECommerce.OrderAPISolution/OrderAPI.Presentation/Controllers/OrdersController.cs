using AutoMapper;
using eCommerce.SharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;
using OrderAPI.Application.DTOs;
using OrderAPI.Application.Interfaces;
using OrderAPI.Application.Services;
using OrderAPI.Domain.Entities;

namespace OrderAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrder orderInterface, IOrderService orderService, IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await orderInterface.GetAllAsync();
            if (!orders.Any())
                return NotFound("No orders found.");
            var orderList = mapper.Map<IEnumerable<OrderDTO>>(orders);
            return (!orderList!.Any()) ? NotFound("No orders found.") : Ok(orderList);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderDTO>> GetOrderById(int id)
        {
            var order = await orderInterface.GetByIdAsync(id);
            if (order is null)
                return NotFound(null);

            var orderDto = mapper.Map<OrderDTO>(order);
            return Ok(orderDto);
        }

        [HttpGet("client/{clientId:int}")]
        public async Task<ActionResult<OrderDTO>> GetClientOrders(int clientId)
        {
            if (clientId <= 0) return BadRequest("Invalid client ID.");

            var orders = await orderService.GetOrdersByClientId(clientId);
            if (!orders.Any())
                return NotFound("No orders found for this client.");
            var orderList = mapper.Map<IEnumerable<OrderDTO>>(orders);
            return Ok(orderList);
        }

        [HttpGet("details/{orderId:int}")]
        public async Task<ActionResult<OrderDetailsDTO>> GetOrderDetails(int orderId)
        {
            if (orderId <= 0) return BadRequest("Invalid order ID.");

            var order = await orderService.GetOrderDetails(orderId);
            if (order is null)
                return NotFound("Order not found.");

            var orderDetailsDto = mapper.Map<OrderDetailsDTO>(order);
            return Ok(orderDetailsDto);
        }

        [HttpPost]
        public async Task<ActionResult<Response>> CreateOrder([FromBody] OrderDTO orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Incomplete Data Sent");

            var orderEntity = mapper.Map<Order>(orderDto);
            var response = await orderInterface.CreateAsync(orderEntity);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpPut]
        public async Task<ActionResult<Response>> UpdateOrder([FromBody] OrderDTO orderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest("Incomplete Data Sent");
            var orderEntity = mapper.Map<Order>(orderDto);
            var response = await orderInterface.UpdateAsync(orderEntity);
            return response.Flag ? Ok(response) : BadRequest(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Response>> DeleteOrder(int id)
        {
            var response = await orderInterface.DeleteAsync(id);
            return response.Flag ? Ok(response) : BadRequest(response);
        }       
    }
}
