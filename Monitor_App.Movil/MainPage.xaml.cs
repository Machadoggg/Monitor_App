using Monitor_App.Movil.Services;

namespace Monitor_App.Movil
{
    public partial class MainPage : ContentPage
    {
        int count = 0;
        private readonly LocationService _locationService;
        private const string ApiUrl = "https://localhost:7054//api/locations"; // Cambiar la URL

        public MainPage()
        {
            InitializeComponent();
            _locationService = new LocationService();
        }



        private async void OnStartClicked(object sender, EventArgs e)
        {
            await CheckAndRequestLocationPermission();
            await _locationService.StartSendingLocation(ApiUrl, TimeSpan.FromSeconds(30));
            StatusLabel.Text = "Enviando ubicación...";
        }

        private void OnStopClicked(object sender, EventArgs e)
        {
            _locationService.StopSendingLocation();
            StatusLabel.Text = "Detenido";
        }

        private async Task CheckAndRequestLocationPermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                {
                    await DisplayAlert("Permiso requerido", "Se necesita permiso de ubicación para esta función", "OK");
                }
            }
        }
    }

}
