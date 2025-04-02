using System.Text.Json.Serialization;

namespace DekatMe.Core.Entities
{
    public class Review
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string BusinessId { get; set; } = string.Empty;
        
        [JsonIgnore]
        public Business? Business { get; set; }
        
        public string UserId { get; set; } = string.Empty;
        
        [JsonIgnore]
        public User? User { get; set; }
        
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public List<string> Photos { get; set; } = new List<string>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        
        // For display purposes
        public string UserName { get; set; } = string.Empty;
        public string UserProfilePicture { get; set; } = string.Empty;
        
        public string GetTimeAgo()
        {
            var span = DateTime.UtcNow - CreatedAt;
            
            if (span.TotalDays > 365)
                return $"{(int)(span.TotalDays / 365)} year{((int)(span.TotalDays / 365) == 1 ? "" : "s")} ago";
            if (span.TotalDays > 30)
                return $"{(int)(span.TotalDays / 30)} month{((int)(span.TotalDays / 30) == 1 ? "" : "s")} ago";
            if (span.TotalDays > 1)
                return $"{(int)span.TotalDays} day{((int)span.TotalDays == 1 ? "" : "s")} ago";
            if (span.TotalHours > 1)
                return $"{(int)span.TotalHours} hour{((int)span.TotalHours == 1 ? "" : "s")} ago";
            if (span.TotalMinutes > 1)
                return $"{(int)span.TotalMinutes} minute{((int)span.TotalMinutes == 1 ? "" : "s")} ago";
            
            return "Just now";
        }
        
        public string[] GetStarRating()
        {
            var stars = new string[5];
            
            for (int i = 0; i < 5; i++)
            {
                stars[i] = i < Rating ? "★" : "☆";
            }
            
            return stars;
        }
    }
}
