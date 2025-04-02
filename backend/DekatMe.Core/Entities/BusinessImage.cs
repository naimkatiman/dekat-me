using System.Text.Json.Serialization;

namespace DekatMe.Core.Entities
{
    public class BusinessImage
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string BusinessId { get; set; } = string.Empty;
        
        [JsonIgnore]
        public Business? Business { get; set; }
        
        public string Url { get; set; } = string.Empty;
        public string Alt { get; set; } = string.Empty;
        public bool IsPrimary { get; set; }
        public int DisplayOrder { get; set; }
        
        public string GetResponsiveImageUrl(int width = 600, int height = 400)
        {
            if (string.IsNullOrEmpty(Url))
                return $"https://placehold.co/{width}x{height}?text=No+Image";
                
            // Check if the URL is from Unsplash, if so, we can use their sizing parameters
            if (Url.Contains("unsplash.com"))
            {
                // Ensure the URL has query parameters
                var baseUrl = Url.Contains("?") ? Url.Split('?')[0] : Url;
                return $"{baseUrl}?w={width}&h={height}&fit=crop&q=80";
            }
            
            // For other image providers, just return the original URL
            return Url;
        }
        
        public string GetImageAltText()
        {
            if (!string.IsNullOrEmpty(Alt))
                return Alt;
                
            // If no alt text is provided, try to generate one from the business name
            if (Business != null)
                return $"Image of {Business.Name}";
                
            return "Business image";
        }
    }
}
