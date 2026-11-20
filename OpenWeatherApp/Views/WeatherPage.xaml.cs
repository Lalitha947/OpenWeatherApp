
using OpenWeatherApp.Services;
using OpenWeatherApp.ViewModels;

namespace OpenWeatherApp.Views;

public partial class WeatherPage : ContentPage
{
    private readonly WeatherViewModel _viewModel;
    private readonly ApiKeyService _apiKeyService;

    public WeatherPage(
        WeatherViewModel viewModel,
        ApiKeyService apiKeyService)
    {
        InitializeComponent();

        _viewModel = viewModel;
        _apiKeyService = apiKeyService;

        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await LoadWeatherDataAsync();
    }

    private async Task LoadWeatherDataAsync()
    {
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        ErrorLabel.Text = string.Empty;

        await _viewModel.LoadWeatherAsync();

        LoadingIndicator.IsRunning = false;
        LoadingIndicator.IsVisible = false;

        if (_viewModel.Weather != null)
        {
            CityLabel.Text = _viewModel.Weather.CityName;

            WeatherIcon.Source = _viewModel.IconUrl;

            TemperatureLabel.Text =
                $"{_viewModel.Weather.Temperature:F1}°C";

            DescriptionLabel.Text =
                _viewModel.Weather.Description;

            FeelsLikeLabel.Text =
                $"Feels like: {_viewModel.Weather.FeelsLike:F1}°C";

            HumidityLabel.Text =
                $"{_viewModel.Weather.Humidity}%";

            WindSpeedLabel.Text =
                $"{_viewModel.Weather.WindSpeed} m/s";
        }

        ErrorLabel.Text = _viewModel.ErrorMessage;
    }

    private async void OnSaveApiKeyClicked(
        object sender, EventArgs e)
    {
        string apiKey = ApiKeyEntry.Text?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            ErrorLabel.Text = "Please enter your API key.";
            return;
        }

        await _apiKeyService.SaveApiKeyAsync(apiKey);

        ErrorLabel.Text = "API key saved successfully!";

        await LoadWeatherDataAsync();
    }

    private async void OnRefreshWeatherClicked(
        object sender, EventArgs e)
    {
        await LoadWeatherDataAsync();
    }
}
