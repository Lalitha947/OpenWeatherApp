
using Microsoft.Extensions.Logging;
using OpenWeatherApp.Services;
using OpenWeatherApp.ViewModels;
using OpenWeatherApp.Views;

namespace OpenWeatherApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        // Register HTTP client
        builder.Services.AddSingleton<HttpClient>();

        // Register services
        builder.Services.AddSingleton<WeatherService>();
        builder.Services.AddSingleton<AppShell>();
        builder.Services.AddSingleton<GeolocationService>();
        builder.Services.AddSingleton<ApiKeyService>();

        // Register configuration interface
        builder.Services.AddSingleton<IConfigurationService>(
            serviceProvider =>
                serviceProvider.GetRequiredService<ApiKeyService>());   

        // Register ViewModel
        builder.Services.AddTransient<WeatherViewModel>();

        // Register Page
        builder.Services.AddTransient<WeatherPage>();

        return builder.Build();
    }
}
