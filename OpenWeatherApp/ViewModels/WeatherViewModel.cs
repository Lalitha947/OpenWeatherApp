
using Microsoft.Maui.Devices.Sensors;
using OpenWeatherApp.Models;
using OpenWeatherApp.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace OpenWeatherApp.ViewModels;

public class WeatherViewModel : INotifyPropertyChanged
{
    private readonly WeatherService _weatherService;
    private readonly GeolocationService _geolocationService;
    private readonly IConfigurationService _configurationService;

    public event PropertyChangedEventHandler? PropertyChanged;

    private WeatherModel? _weather;

    public WeatherModel? Weather
    {
        get => _weather;
        set
        {
            _weather = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(CityName));
            OnPropertyChanged(nameof(Temperature));
            OnPropertyChanged(nameof(WeatherDescription));
            OnPropertyChanged(nameof(Humidity));
            OnPropertyChanged(nameof(WindSpeed));
        }
    }

    public string CityName => Weather?.CityName ?? "Loading...";

    public double Temperature => Weather?.Temperature ?? 0;

    public string WeatherDescription =>
        Weather?.Description ?? "Weather information unavailable";

    public int Humidity => Weather?.Humidity ?? 0;

    public double WindSpeed => Weather?.WindSpeed ?? 0;

    private bool _isLoading;

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    private string _errorMessage = string.Empty;

    public string ErrorMessage
    {
        get => _errorMessage;
        set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public ICommand RefreshWeatherCommand { get; }

    public ICommand SearchWeatherCommand { get; }

    public WeatherViewModel(
        WeatherService weatherService,
        GeolocationService geolocationService,
        IConfigurationService configurationService)
    {
        _weatherService = weatherService;
        _geolocationService = geolocationService;
        _configurationService = configurationService;

        RefreshWeatherCommand = new Command(async () =>
            await LoadWeatherAsync());

        SearchWeatherCommand = new Command(async () =>
            await SearchWeatherAsync());
    }

    public async Task LoadWeatherAsync()
    {
        try
        {
            IsLoading = true;
            ErrorMessage = string.Empty;

            string? apiKey = await _configurationService.GetApiKeyAsync();

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                ErrorMessage = "OpenWeather API key not found. Please configure your API key.";
                return;
            }

            Location? location =
                await _geolocationService.GetCurrentLocationAsync();

            if (location == null)
            {
                ErrorMessage = "Unable to retrieve your location.";
                return;
            }

            WeatherResponse? response =
                await _weatherService.GetWeatherAsync(
                    location.Latitude,
                    location.Longitude,
                    apiKey);

            if (response?.Main == null ||
                response.Wind == null ||
                response.Weather == null ||
                response.Weather.Count == 0)
            {
                ErrorMessage = "Unable to retrieve weather information.";
                return;
            }

            Weather = new WeatherModel
            {
                CityName = response.Name,
                Temperature = response.Main.Temp,
                FeelsLike = response.Main.Feels_Like,
                Humidity = response.Main.Humidity,
                WindSpeed = response.Wind.Speed,
                Description = response.Weather[0].Description,
                Icon = response.Weather[0].Icon
            };
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Something went wrong: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task SearchWeatherAsync()
    {
        ErrorMessage = "City search will be connected in the next step.";
        await Task.CompletedTask;
    }

    protected void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
