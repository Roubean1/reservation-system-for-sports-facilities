using Blazored.LocalStorage;
using Frontend.DTO;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

public class AuthService
{
    private readonly ILocalStorageService _localStorage;

    public AuthService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }



    public async Task<string?> GetToken()
    {
        return await _localStorage.GetItemAsync<string>("token");
    }


    public async Task<UserResponseDto?> GetUser()
    {
        return await _localStorage.GetItemAsync<UserResponseDto>("user");
    }


    public async Task<bool> IsLoggedIn()
    {
        var token = await _localStorage.GetItemAsync<string>("token");
        return !string.IsNullOrEmpty(token);
    }

    public async Task Logout(HttpClient http)
    {
        await _localStorage.RemoveItemAsync("token");
        await _localStorage.RemoveItemAsync("user");

        http.DefaultRequestHeaders.Authorization = null;
    }

}