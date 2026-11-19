
namespace OpenWeatherApp.Services;

public class ApiKeyService : IConfigurationService
{
    private const string ApiKeyName = "OpenWeatherApiKey";

    public async Task SaveApiKeyAsync(string apiKey)
    {
        await SecureStorage.Default.SetAsync(ApiKeyName, apiKey);
    }

    public async Task<string?> GetApiKeyAsync()
    {
        return await SecureStorage.Default.GetAsync(ApiKeyName);
    }

    public void RemoveApiKey()
    {
        SecureStorage.Default.Remove(ApiKeyName);
    }
}
