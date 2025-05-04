namespace Monitor_App.Web.Models
{
    public class LocationData
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double? Altitude { get; set; }
        public double? Accuracy { get; set; }
        public double? Speed { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
