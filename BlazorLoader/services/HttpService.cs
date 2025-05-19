using System.Net.Http;
using System.Text.Json;

public class HttpService
{

    private readonly HttpClient _httpClient;
    public HttpService(IHttpClientFactory factory)
    {
        _httpClient = factory.CreateClient();
    }
    public async Task<T> FetchAsync<T>(string url)
    {
        await Task.Delay(2000);
        var result = await _httpClient.GetAsync(url);
        result.EnsureSuccessStatusCode();
        var content = await result.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content)!;
    }
}
