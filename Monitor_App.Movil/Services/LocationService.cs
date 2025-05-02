using System.Diagnostics;
using System.Text.Json;
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;

namespace Monitor_App.Movil.Services
{
    public class LocationService
    {
        private readonly HttpClient _httpClient;
        private bool _isSendingLocation;
        private CancellationTokenSource _cts;
        private string _deviceId;

        public LocationService()
        {
            _httpClient = new HttpClient();
            _deviceId = DeviceInfo.Model + DeviceInfo.Platform; // Identificador único del dispositivo
        }

        public async Task StartSendingLocation(string apiUrl, TimeSpan interval)
        {
            if (_isSendingLocation)
                return;

            _isSendingLocation = true;
            _cts = new CancellationTokenSource();

            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    await SendCurrentLocation(apiUrl);
                    await Task.Delay(interval, _cts.Token);
                }
            }
            catch (TaskCanceledException)
            {
                Debug.WriteLine("Envío de ubicación cancelado");
            }
            finally
            {
                _isSendingLocation = false;
            }
        }

        public void StopSendingLocation()
        {
            _cts?.Cancel();
        }

        private async Task SendCurrentLocation(string apiUrl)
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    var locationData = new
                    {
                        DeviceId = _deviceId,
                        Latitude = location.Latitude,
                        Longitude = location.Longitude,
                        Altitude = location.Altitude,
                        Accuracy = location.Accuracy,
                        Speed = location.Speed,
                        Timestamp = DateTime.UtcNow
                    };

                    var json = JsonSerializer.Serialize(locationData);
                    var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                    var response = await _httpClient.PostAsync(apiUrl, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        Debug.WriteLine($"Error al enviar ubicación: {response.StatusCode}");
                    }
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Debug.WriteLine($"GPS no soportado: {fnsEx.Message}");
            }
            catch (PermissionException pEx)
            {
                Debug.WriteLine($"Permisos no concedidos: {pEx.Message}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error obteniendo/enviando ubicación: {ex.Message}");
            }
        }
    }
}
