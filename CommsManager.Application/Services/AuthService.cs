using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using CommsManager.Application.DTOs.Auth;
using CommsManager.Application.Interfaces;
using CommsManager.Core.Interfaces;
using CommsManager.Core.Entities;
using CommsManager.Core.Enums;
using Microsoft.Extensions.Configuration;

namespace CommsManager.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _users;
    private readonly IUnitOfWork _uow;
    private readonly IConfiguration _configuration;

    public AuthService(IUserRepository users, IUnitOfWork uow, IConfiguration configuration)
    {
        _users = users;
        _uow = uow;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto, CancellationToken cancellationToken = default)
    {
        var existing = await _users.GetByEmailAsync(dto.Email, cancellationToken);
        if (existing != null)
            throw new InvalidOperationException("User with this email already exists");

        CreatePasswordHash(dto.Password, out var hash, out var salt);
        var user = new User(dto.Email, hash, salt);

        // Optionally create Customer domain object and link it
        if (!string.IsNullOrWhiteSpace(dto.Name))
        {
            var customer = new CommsManager.Core.Entities.Customer(dto.Name);
            await _uow.Customers.AddAsync(customer, cancellationToken);
            user.CustomerId = customer.Id;
        }

        await _users.AddAsync(user, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);

        var token = GenerateJwtToken(user);
        return new AuthResponseDto { Token = token, ExpiresAt = DateTime.UtcNow.AddMinutes(GetExpiryMinutes()) };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByEmailAsync(dto.Email, cancellationToken);
        if (user == null)
            throw new InvalidOperationException("Invalid credentials");

        if (!VerifyPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
            throw new InvalidOperationException("Invalid credentials");

        var token = GenerateJwtToken(user);
        return new AuthResponseDto { Token = token, ExpiresAt = DateTime.UtcNow.AddMinutes(GetExpiryMinutes()) };
    }

    public async Task PromoteToCreatorAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(userId, cancellationToken);
        if (user == null)
            throw new InvalidOperationException("User not found");

        if (user.Role == UserRole.Creator)
            return; // already creator

        // create ArtistProfile domain entity
        var profile = new CommsManager.Core.Entities.ArtistProfile(user.Email);
        await _uow.ArtistProfiles.AddAsync(profile, cancellationToken);

        user.PromoteToCreator(profile.Id);
        await _users.UpdateAsync(user, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
    }

    // --- helpers ---
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        using var hmac = new HMACSHA512(storedSalt);
        var computed = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computed.SequenceEqual(storedHash);
    }

    private string GenerateJwtToken(User user)
    {
        var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key not configured");
        var issuer = _configuration["Jwt:Issuer"] ?? "CommsManager";

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: issuer,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(GetExpiryMinutes()),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private int GetExpiryMinutes()
    {
        if (int.TryParse(_configuration["Jwt:ExpiryMinutes"], out var minutes))
            return minutes;
        return 60;
    }
}
