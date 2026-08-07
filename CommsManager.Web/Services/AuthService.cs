using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommsManager.Application.DTOs.Auth;

namespace CommsManager.Web.Services;

public class AuthService
{
    private readonly HttpClient _http;

    public AuthService(HttpClient http)
    {
        _http = http;
    }

    public string? Token { get; private set; }
    public string? Email { get; private set; }

    public bool IsAuthenticated => !string.IsNullOrWhiteSpace(Token);

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        var resp = await _http.PostAsJsonAsync("api/auth/register", dto);
        resp.EnsureSuccessStatusCode();
        var result = await resp.Content.ReadFromJsonAsync<AuthResponseDto>();
        SetToken(result?.Token);
        return result!;
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var resp = await _http.PostAsJsonAsync("api/auth/login", dto);
        resp.EnsureSuccessStatusCode();
        var result = await resp.Content.ReadFromJsonAsync<AuthResponseDto>();
        SetToken(result?.Token);
        return result!;
    }

    public void Logout()
    {
        Token = null;
        Email = null;
        _http.DefaultRequestHeaders.Authorization = null;
    }

    private void SetToken(string? token)
    {
        Token = token;
        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        else
        {
            _http.DefaultRequestHeaders.Authorization = null;
        }
    }
}
