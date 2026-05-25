using AutoMapper;
using CommsManager.Core.Entities;
using CommsManager.Application.DTOs.Order;
using CommsManager.Application.DTOs.Customer;
using CommsManager.Application.DTOs.ArtistProfile;

namespace CommsManager.Application.Common;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Order mappings
        CreateMap<Order, OrderResponseDto>()
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => "Unknown"))
            .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => "Unknown"))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Price.Currency));

        CreateMap<CreateOrderDto, Order>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Price, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

        // Customer mappings
        CreateMap<Customer, CustomerResponseDto>()
            .ForMember(dest => dest.OrderCount, opt => opt.MapFrom(src => src.Orders != null ? src.Orders.Count : 0));

        CreateMap<CreateCustomerDto, Customer>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());

        // ArtistProfile mappings
        CreateMap<ArtistProfile, ArtistProfileResponseDto>()
            .ForMember(dest => dest.CommissionCount, opt => opt.MapFrom(src => src.Commissions != null ? src.Commissions.Count : 0));

        CreateMap<CreateArtistProfileDto, ArtistProfile>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
            .ForMember(dest => dest.DomainEvents, opt => opt.Ignore());
    }
}
