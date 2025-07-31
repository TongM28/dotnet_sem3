namespace MogoDbProductAPI.Domain.DTOs
{
    public class UpdateOrderDto
    {
        public List<OrderItemDto> Items { get; set; } = new();
        public string Status { get; set; } = "Pending";
    }
}
