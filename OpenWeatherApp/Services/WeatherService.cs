
using System.Text.Json;
using OpenWeatherApp.Models;

namespace OpenWeatherApp.Services;

public class WeatherService
{
    private readonly HttpClient _httpClient;

    public WeatherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherResponse?> GetWeatherAsync(
        double latitude,
        double longitude,
        string apiKey)
    {
        string url = $"https://api.openweathermap.org/data/2.5/weather" +
                     $"?lat={latitude}&lon={longitude}" +
                     $"&appid={apiKey}&units=metric";

        HttpResponseMessage response =
            await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<WeatherResponse>(
            json,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
    }
}
