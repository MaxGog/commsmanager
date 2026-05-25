using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using CommsManager.Application.Common;
using CommsManager.Application.Services;
using CommsManager.Application.Validators.Order;
using CommsManager.Application.Validators.Customer;
using CommsManager.Application.Validators.ArtistProfile;
using CommsManager.Application.DTOs.Order;
using CommsManager.Application.DTOs.Customer;
using CommsManager.Application.DTOs.ArtistProfile;

namespace CommsManager.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // Validators
        services.AddScoped<IValidator<CreateOrderDto>, CreateOrderValidator>();
        services.AddScoped<IValidator<UpdateOrderDto>, UpdateOrderValidator>();
        services.AddScoped<IValidator<CreateCustomerDto>, CreateCustomerValidator>();
        services.AddScoped<IValidator<CreateArtistProfileDto>, CreateArtistProfileValidator>();

        // Services
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IArtistProfileService, ArtistProfileService>();

        return services;
    }
}
