using AutoMapper;
using Order.WebAPI.DTOs;
using Order.WebAPI.Models;

namespace Order.WebAPI.Mapping;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<OrderItemDto, OrderItem>().ReverseMap();
    }
}


public static class ObjectMapper
{
    private static readonly Lazy<IMapper> _lazy = new(() =>
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<OrderProfile>();
        });

        return config.CreateMapper();
    });


    public static IMapper Mapper => _lazy.Value;  
}
