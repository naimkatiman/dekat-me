using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;

namespace DekatMe.Api.Utilities
{
    public class GeocodingService : IGeocodingService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<GeocodingService> _logger;
        private readonly string _apiKey;

        public GeocodingService(HttpClient httpClient, IMemoryCache cache, ILogger<GeocodingService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _cache = cache;
            _logger = logger;
            _apiKey = configuration["GeocodingApiKey"] ?? throw new InvalidOperationException("Geocoding API key not found in configuration");
        }

        public async Task<GeocodingResult?> GeocodeAddressAsync(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return null;
            }

            // Check cache first
            string cacheKey = $"geocode_{address}";
            if (_cache.TryGetValue(cacheKey, out GeocodingResult cachedResult))
            {
                _logger.LogInformation("Cache hit for address: {Address}", address);
                return cachedResult;
            }

            try
            {
                var encodedAddress = Uri.EscapeDataString(address);
                var response = await _httpClient.GetAsync($"https://maps.googleapis.com/maps/api/geocode/json?address={encodedAddress}&key={_apiKey}");
                
                if (response.IsSuccessStatusCode)
                {
                    var geocodingResponse = await response.Content.ReadFromJsonAsync<GoogleGeocodingResponse>();
                    
                    if (geocodingResponse?.Status == "OK" && geocodingResponse.Results.Any())
                    {
                        var location = geocodingResponse.Results[0].Geometry.Location;
                        var result = new GeocodingResult
                        {
                            Latitude = location.Lat,
                            Longitude = location.Lng,
                            FormattedAddress = geocodingResponse.Results[0].FormattedAddress
                        };
                        
                        // Cache the result for 24 hours
                        _cache.Set(cacheKey, result, TimeSpan.FromHours(24));
                        
                        return result;
                    }
                    else
                    {
                        _logger.LogWarning("Geocoding failed for address {Address}. Status: {Status}", 
                            address, geocodingResponse?.Status ?? "Unknown");
                    }
                }
                else
                {
                    _logger.LogError("Geocoding API returned status code {StatusCode} for address {Address}", 
                        response.StatusCode, address);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error geocoding address {Address}", address);
            }
            
            return null;
        }

        public async Task<string?> ReverseGeocodeAsync(double latitude, double longitude)
        {
            // Check cache first
            string cacheKey = $"reverse_geocode_{latitude}_{longitude}";
            if (_cache.TryGetValue(cacheKey, out string cachedAddress))
            {
                _logger.LogInformation("Cache hit for coordinates: {Latitude}, {Longitude}", latitude, longitude);
                return cachedAddress;
            }

            try
            {
                var response = await _httpClient.GetAsync($"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longitude}&key={_apiKey}");
                
                if (response.IsSuccessStatusCode)
                {
                    var geocodingResponse = await response.Content.ReadFromJsonAsync<GoogleGeocodingResponse>();
                    
                    if (geocodingResponse?.Status == "OK" && geocodingResponse.Results.Any())
                    {
                        var address = geocodingResponse.Results[0].FormattedAddress;
                        
                        // Cache the result for 24 hours
                        _cache.Set(cacheKey, address, TimeSpan.FromHours(24));
                        
                        return address;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reverse geocoding coordinates {Latitude}, {Longitude}", latitude, longitude);
            }
            
            return null;
        }
    }

    public interface IGeocodingService
    {
        Task<GeocodingResult?> GeocodeAddressAsync(string address);
        Task<string?> ReverseGeocodeAsync(double latitude, double longitude);
    }

    public class GeocodingResult
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string FormattedAddress { get; set; } = string.Empty;
    }

    public class GoogleGeocodingResponse
    {
        public string Status { get; set; } = string.Empty;
        public List<GoogleGeocodingResult> Results { get; set; } = new();
    }

    public class GoogleGeocodingResult
    {
        public string FormattedAddress { get; set; } = string.Empty;
        public GoogleGeometry Geometry { get; set; } = new();
    }

    public class GoogleGeometry
    {
        public GoogleLocation Location { get; set; } = new();
    }

    public class GoogleLocation
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
