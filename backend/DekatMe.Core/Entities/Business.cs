using System.Text.Json.Serialization;

namespace DekatMe.Core.Entities
{
    public class Business
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public string CategoryId { get; set; } = string.Empty;

        [JsonIgnore]
        public Category? Category { get; set; }
        
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = new List<string>();
        public List<BusinessHour> Hours { get; set; } = new List<BusinessHour>();
        public List<BusinessImage> Images { get; set; } = new List<BusinessImage>();
        
        [JsonIgnore]
        public List<Review> Reviews { get; set; } = new List<Review>();
        
        public bool IsFeatured { get; set; }
        public bool IsPremium { get; set; }
        public bool IsVerified { get; set; }
        public double Rating { get; set; }
        public int ReviewsCount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        
        public double DistanceFromCurrentLocation { get; set; }
        
        public string FormattedAddress => $"{Address}, {City} {PostalCode}".Trim();
        
        public string GetPrimaryImageUrl()
        {
            var primaryImage = Images.FirstOrDefault(i => i.IsPrimary);
            return primaryImage?.Url ?? "https://placehold.co/600x400?text=No+Image";
        }
        
        public bool IsOpenNow()
        {
            var now = DateTime.Now;
            var dayOfWeek = (int)now.DayOfWeek;
            var todayHours = Hours.FirstOrDefault(h => h.DayOfWeek == dayOfWeek);
            
            if (todayHours == null || todayHours.IsClosed)
                return false;
                
            var currentTime = now.TimeOfDay;
            return todayHours.OpenTime <= currentTime && currentTime <= todayHours.CloseTime;
        }
        
        public string GetBusinessHoursFormatted(int dayOfWeek)
        {
            var hours = Hours.FirstOrDefault(h => h.DayOfWeek == dayOfWeek);
            
            if (hours == null || hours.IsClosed)
                return "Closed";
                
            return $"{hours.OpenTime.ToString(@"hh\:mm")} - {hours.CloseTime.ToString(@"hh\:mm")}";
        }
    }
}
