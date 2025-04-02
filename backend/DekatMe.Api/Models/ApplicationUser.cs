using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DekatMe.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        public string ProfilePicture { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? LastLogin { get; set; }
        
        public List<Review> Reviews { get; set; } = new List<Review>();
        
        public List<Business> FavoriteBusinesses { get; set; } = new List<Business>();
    }
}
