
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;

namespace OpenWeatherApp.Services;

public class GeolocationService
{
    public async Task<Location?> GetCurrentLocationAsync()
    {
        try
        {
            // Check location permission
            PermissionStatus status =
                await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            // Request permission if not already granted
            if (status != PermissionStatus.Granted)
            {
                status =
                    await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            // Stop if permission was denied
            if (status != PermissionStatus.Granted)
            {
                return null;
            }

            // Get the device's current location
            Location? location =
                await Geolocation.Default.GetLocationAsync(
                    new GeolocationRequest(
                        GeolocationAccuracy.Medium,
                        TimeSpan.FromSeconds(10)));

            return location;
        }
        catch (FeatureNotSupportedException)
        {
            return null;
        }
        catch (FeatureNotEnabledException)
        {
            return null;
        }
        catch (PermissionException)
        {
            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
