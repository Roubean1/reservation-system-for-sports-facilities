using System.Net.Http.Headers;
using Blazored.LocalStorage;

public class JwtHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public JwtHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Vytáhneme token z LocalStorage
        var token = await _localStorage.GetItemAsync<string>("token");

        if (!string.IsNullOrEmpty(token))
        {
            // Přidáme ho do hlavičky Bearer
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}