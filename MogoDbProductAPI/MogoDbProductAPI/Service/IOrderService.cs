using MogoDbProductAPI.Domain.DTOs;

namespace MogoDbProductAPI.Service
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderDto dto);
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderDto> GetOrderByIdAsync(string id);
        Task<OrderDto> UpdateOrderAsync(string id, UpdateOrderDto dto);
        Task DeleteOrderAsync(string id);
    }
}
