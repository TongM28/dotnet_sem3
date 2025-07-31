    using MongoDB.Driver;
using MogoDbProductAPI.Domain.DTOs;
using MogoDbProductAPI.Domain.Models;
using AutoMapper;

namespace MogoDbProductAPI.Service
{
    public class OrderService : IOrderService
    {
        private readonly IMongoCollection<Order> _orders;
        private readonly IMapper _mapper;

        public OrderService(IMongoDatabase database, IMapper mapper)
        {
            _orders = database.GetCollection<Order>("Orders");
            _mapper = mapper;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto)
        {
            var order = new Order
            {
                UserId = dto.UserId,
                Items = dto.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity
                }).ToList(),
                TotalAmount = 0, // Tính nếu cần
                CreatedAt = DateTime.UtcNow,
                Status = "Pending"
            };

            await _orders.InsertOneAsync(order);
            return _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orders.Find(_ => true).ToListAsync();
            return orders.Select(o => _mapper.Map<OrderDto>(o));
        }

        public async Task<OrderDto> GetOrderByIdAsync(string id)
        {
            var order = await _orders.Find(o => o.Id == id).FirstOrDefaultAsync();
            if (order == null) throw new Exception("Order not found");

            return _mapper.Map<OrderDto>(order);
        }

        public async Task<OrderDto> UpdateOrderAsync(string id, UpdateOrderDto dto)
        {
            var update = Builders<Order>.Update
                .Set(o => o.Items, dto.Items.Select(i => new OrderItem { ProductId = i.ProductId, Quantity = i.Quantity }).ToList())
                .Set(o => o.Status, dto.Status);

            var result = await _orders.FindOneAndUpdateAsync(
                o => o.Id == id,
                update,
                new FindOneAndUpdateOptions<Order> { ReturnDocument = ReturnDocument.After }
            );

            if (result == null) throw new Exception("Order not found");
            return _mapper.Map<OrderDto>(result);
        }

        public async Task DeleteOrderAsync(string id)
        {
            var result = await _orders.DeleteOneAsync(o => o.Id == id);
            if (result.DeletedCount == 0) throw new Exception("Order not found");
        }
    }
}
