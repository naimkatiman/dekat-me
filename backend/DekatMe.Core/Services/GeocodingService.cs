using System.Text.Json;

namespace DekatMe.Core.Services
{
    public class GeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly ILogger<GeocodingService> _logger;

        public GeocodingService(HttpClient httpClient, string apiKey, ILogger<GeocodingService> logger)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
            _logger = logger;
        }

        public async Task<(double Latitude, double Longitude)?> GeocodeAddressAsync(string address)
        {
            try
            {
                var encodedAddress = Uri.EscapeDataString(address);
                var url = $"https://maps.googleapis.com/maps/api/geocode/json?address={encodedAddress}&key={_apiKey}";
                
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GoogleGeocodingResponse>(content);
                
                if (result?.Status != "OK" || result.Results.Length == 0)
                {
                    _logger.LogWarning("Geocoding failed for address: {Address}, Status: {Status}", 
                        address, result?.Status ?? "Unknown");
                    return null;
                }
                
                var location = result.Results[0].Geometry.Location;
                return (location.Lat, location.Lng);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geocoding address: {Address}", address);
                return null;
            }
        }

        public async Task<string?> ReverseGeocodeAsync(double latitude, double longitude)
        {
            try
            {
                var url = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key={_apiKey}";
                
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GoogleGeocodingResponse>(content);
                
                if (result?.Status != "OK" || result.Results.Length == 0)
                {
                    _logger.LogWarning("Reverse geocoding failed for coordinates: {Latitude}, {Longitude}, Status: {Status}", 
                        latitude, longitude, result?.Status ?? "Unknown");
                    return null;
                }
                
                return result.Results[0].FormattedAddress;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reverse geocoding coordinates: {Latitude}, {Longitude}", latitude, longitude);
                return null;
            }
        }

        public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double EarthRadiusKm = 6371;
            
            var dLat = DegreesToRadians(lat2 - lat1);
            var dLon = DegreesToRadians(lon2 - lon1);
            
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegreesToRadians(lat1)) * Math.Cos(DegreesToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EarthRadiusKm * c;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
    }

    // JSON response classes for Google Geocoding API
    public class GoogleGeocodingResponse
    {
        public string Status { get; set; } = string.Empty;
        public GoogleGeocodingResult[] Results { get; set; } = Array.Empty<GoogleGeocodingResult>();
    }

    public class GoogleGeocodingResult
    {
        public string FormattedAddress { get; set; } = string.Empty;
        public GoogleGeometry Geometry { get; set; } = new GoogleGeometry();
    }

    public class GoogleGeometry
    {
        public GoogleLocation Location { get; set; } = new GoogleLocation();
    }

    public class GoogleLocation
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
