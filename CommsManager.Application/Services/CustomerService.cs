using AutoMapper;
using FluentValidation;
using CommsManager.Application.DTOs.Customer;
using CommsManager.Application.Exceptions;
using CommsManager.Core.Entities;
using CommsManager.Core.Interfaces;

namespace CommsManager.Application.Services;

public interface ICustomerService
{
    Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerDto dto);
    Task<CustomerResponseDto> GetCustomerByIdAsync(Guid id);
    Task<List<CustomerResponseDto>> GetAllCustomersAsync();
    Task<CustomerResponseDto> UpdateCustomerAsync(Guid id, UpdateCustomerDto dto);
    Task DeleteCustomerAsync(Guid id);
}

public class CustomerService : ICustomerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateCustomerDto> _createValidator;

    public CustomerService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IValidator<CreateCustomerDto> createValidator)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _createValidator = createValidator;
    }

    public async Task<CustomerResponseDto> CreateCustomerAsync(CreateCustomerDto dto)
    {
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());
            throw new Exceptions.ValidationException(errors);
        }

        var customer = new Customer(dto.Name)
        {
            Description = dto.Description
        };

        // Add email if provided
        if (!string.IsNullOrEmpty(dto.Email))
            customer.AddEmail(dto.Email, null, "Primary");

        // Add phone if provided
        if (!string.IsNullOrEmpty(dto.Phone))
            customer.AddPhone(dto.Phone, null, "Primary");

        await _unitOfWork.Customers.AddAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(customer);
    }

    public async Task<CustomerResponseDto> GetCustomerByIdAsync(Guid id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        if (customer == null)
            throw new NotFoundException("Customer", id);

        return MapToResponse(customer);
    }

    public async Task<List<CustomerResponseDto>> GetAllCustomersAsync()
    {
        var customers = await _unitOfWork.Customers.GetAllAsync();
        return customers.Select(MapToResponse).ToList();
    }

    public async Task<CustomerResponseDto> UpdateCustomerAsync(Guid id, UpdateCustomerDto dto)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        if (customer == null)
            throw new NotFoundException("Customer", id);

        if (!string.IsNullOrEmpty(dto.Name))
            customer.Name = dto.Name;

        if (!string.IsNullOrEmpty(dto.Description))
            customer.Description = dto.Description;

        if (dto.IsActive.HasValue)
        {
            if (dto.IsActive.Value)
                customer.Activate();
            else
                customer.Deactivate();
        }

        await _unitOfWork.Customers.UpdateAsync(customer);
        await _unitOfWork.SaveChangesAsync();

        return MapToResponse(customer);
    }

    public async Task DeleteCustomerAsync(Guid id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        if (customer == null)
            throw new NotFoundException("Customer", id);

        await _unitOfWork.Customers.DeleteAsync(customer);
        await _unitOfWork.SaveChangesAsync();
    }

    private CustomerResponseDto MapToResponse(Customer customer)
    {
        return new CustomerResponseDto
        {
            Id = customer.Id,
            Name = customer.Name,
            Email = customer.Emails?.FirstOrDefault()?.EmailAdress,
            Phone = customer.Phones?.FirstOrDefault()?.NumberPhone,
            Description = customer.Description,
            OrderCount = customer.Orders?.Count ?? 0,
            IsActive = customer.IsActive,
            CreatedDate = customer.CreatedDate
        };
    }
}
