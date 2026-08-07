using Xunit;
using CommsManager.Application.Services;
using CommsManager.Core.Entities;
using CommsManager.Core.Interfaces;
using Moq;
using Microsoft.Extensions.Configuration;
using CommsManager.Application.DTOs.Auth;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace CommsManager.Application.Tests;

public class AuthenticationTests
{
    [Fact]
    public async Task Register_ShouldCreateUserAndReturnToken()
    {
        var userRepo = new Mock<IUserRepository>();
        var uow = new Mock<IUnitOfWork>();
        var config = new ConfigurationBuilder().AddInMemoryCollection(new[] {
            // key must be at least 256 bits for HS256 (32 bytes). Use a sufficiently long test key.
            new KeyValuePair<string,string?>("Jwt:Key", "ThisIsATestKeyWithEnoughLengthForHS256_0123456789ABCDEF"),
            new KeyValuePair<string,string?>("Jwt:Issuer", "TestIssuer"),
            new KeyValuePair<string,string?>("Jwt:ExpiryMinutes", "60")
        }).Build();

        userRepo.Setup(r => r.GetByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((CommsManager.Core.Entities.User?)null);
        userRepo.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask).Verifiable();
        uow.Setup(u => u.Customers.AddAsync(It.IsAny<CommsManager.Core.Entities.Customer>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
        uow.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var service = new AuthService(userRepo.Object, uow.Object, config);
        var dto = new RegisterDto { Email = "test@example.com", Password = "Password1!", Name = "Tester" };

        var result = await service.RegisterAsync(dto);

        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        var config = new ConfigurationBuilder().AddInMemoryCollection(new[] {
            new KeyValuePair<string,string?>("Jwt:Key", "ThisIsATestKeyWithEnoughLengthForHS256_0123456789ABCDEF"),
            new KeyValuePair<string,string?>("Jwt:Issuer", "TestIssuer"),
            new KeyValuePair<string,string?>("Jwt:ExpiryMinutes", "60")
        }).Build();

        // Create password hash for known password
        using var hmac = new System.Security.Cryptography.HMACSHA512();
        var salt = hmac.Key;
        var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("Password1!"));

        var user = new User("login@example.com", hash, salt);

        var userRepo = new Mock<IUserRepository>();
        userRepo.Setup(r => r.GetByEmailAsync("login@example.com", It.IsAny<CancellationToken>())).ReturnsAsync(user);

        var uow = new Mock<IUnitOfWork>();

        var service = new AuthService(userRepo.Object, uow.Object, config);

        var dto = new LoginDto { Email = "login@example.com", Password = "Password1!" };

        var result = await service.LoginAsync(dto);

        Assert.NotNull(result);
        Assert.False(string.IsNullOrWhiteSpace(result.Token));
    }
}
