
namespace OpenWeatherApp.Models;

public class WeatherResponse
{
    public string Name { get; set; } = string.Empty;

    public MainWeather? Main { get; set; }

    public Wind? Wind { get; set; }

    public List<WeatherInfo>? Weather { get; set; }
}

public class MainWeather
{
    public double Temp { get; set; }

    public double Feels_Like { get; set; }

    public int Humidity { get; set; }
}

public class Wind
{
    public double Speed { get; set; }
}

public class WeatherInfo
{
    public string Description { get; set; } = string.Empty;

    public string Icon { get; set; } = string.Empty;
}
