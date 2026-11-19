using System;
using System.Collections.Generic;
using System.Text;



namespace OpenWeatherApp.Services;

public interface IConfigurationService
{
    Task<string?> GetApiKeyAsync();
}


