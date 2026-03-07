using AutoMapper;
using OrderAPI.Application.DTOs;
using OrderAPI.Application.Interfaces;
using Polly;
using Polly.Registry;
using System.Net.Http.Json;

namespace OrderAPI.Application.Services
{
    public class OrderService(IOrder orderInterface, HttpClient httpClient, ResiliencePipelineProvider<string> resiliencePipeline, IMapper mapper) : IOrderService
    {
        public async Task<ProductDTO> GetProduct(int productId)
        {
            var getProduct = await httpClient.GetAsync($"/api/products/{productId}");
            if (!getProduct.IsSuccessStatusCode)
                return null!;

            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product!;
        }

        public async Task<AppUserDTO> GetUser(int userId)
        {
            var getUser = await httpClient.GetAsync($"/api/users/{userId}");
            if (!getUser.IsSuccessStatusCode)
                return null!;
           
            var user = await getUser.Content.ReadFromJsonAsync<AppUserDTO>();
            return user!;
        }
        public async Task<OrderDetailsDTO> GetOrderDetails(int orderId)
        {
            var order = await orderInterface.GetByIdAsync(orderId);
            if (order is null || order!.Id <= 0)
                return null!;

            var retryPipeline = resiliencePipeline.GetPipeline("retry-pipeline");

            var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));

            var appUserDTO = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientId));

            return new OrderDetailsDTO(
                order.Id,
                productDTO.Id,
                appUserDTO.Id,
                appUserDTO.Name,
                appUserDTO.Email,
                appUserDTO.Address,
                appUserDTO.TelephoneNumber, 
                productDTO.Name,
                order.PurchasedQuantity,
                productDTO.Price,
                productDTO.Quantity * order.PurchasedQuantity,
                order.OrderedDate
            );
        }

        public async Task<IEnumerable<OrderDTO>> GetOrdersByClientId(int clientId)
        {
            var orders = await orderInterface.GetOrdersAsync(o => o.ClientId == clientId);
            if(!orders.Any())
                return null!;

            var _orders = mapper.Map<IEnumerable<OrderDTO>>(orders);
            return _orders!;
        }
    }
}
