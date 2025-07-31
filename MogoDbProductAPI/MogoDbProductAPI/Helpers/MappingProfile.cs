using AutoMapper;
using MogoDbProductAPI.Domain.Contracts;
using MogoDbProductAPI.Domain.DTOs;
using MogoDbProductAPI.Domain.Model;
using MogoDbProductAPI.Domain.Models;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Order, OrderDto>().ReverseMap();
        CreateMap<CreateOrderDto, Order>();
        CreateMap<UpdateOrderDto, Order>();

        // Tùy theo số DTO và Entity bạn có thể mở rộng thêm
        CreateMap<Product, ProductDto>().ReverseMap();
    }
}
