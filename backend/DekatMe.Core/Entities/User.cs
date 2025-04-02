using System.Text.Json.Serialization;

namespace DekatMe.Core.Entities
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLogin { get; set; }
        
        [JsonIgnore]
        public List<Review> Reviews { get; set; } = new List<Review>();
        
        [JsonIgnore]
        public List<Business> FavoriteBusinesses { get; set; } = new List<Business>();
        
        public string FullName => $"{FirstName} {LastName}".Trim();
        
        public string GetInitials()
        {
            if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName))
                return $"{FirstName[0]}{LastName[0]}";
                
            if (!string.IsNullOrEmpty(FirstName))
                return FirstName[0].ToString();
                
            if (!string.IsNullOrEmpty(LastName))
                return LastName[0].ToString();
                
            if (!string.IsNullOrEmpty(UserName))
                return UserName[0].ToString();
                
            return "U";
        }
        
        public string GetProfilePictureOrDefault()
        {
            return !string.IsNullOrEmpty(ProfilePicture) 
                ? ProfilePicture 
                : $"https://ui-avatars.com/api/?name={Uri.EscapeDataString(FullName)}&background=random";
        }
    }
}
