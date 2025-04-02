using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DekatMe.Api.Models
{
    public class Business
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string ShortDescription { get; set; } = string.Empty;
        
        [Required]
        public string Address { get; set; } = string.Empty;
        
        [MaxLength(50)]
        public string City { get; set; } = string.Empty;
        
        [MaxLength(10)]
        public string PostalCode { get; set; } = string.Empty;
        
        [Required]
        public string CategoryId { get; set; } = string.Empty;
        
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }
        
        public double Latitude { get; set; }
        
        public double Longitude { get; set; }
        
        [MaxLength(20)]
        public string Phone { get; set; } = string.Empty;
        
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        public string Website { get; set; } = string.Empty;
        
        public List<string> Tags { get; set; } = new List<string>();
        
        public List<BusinessHour> Hours { get; set; } = new List<BusinessHour>();
        
        public List<BusinessImage> Images { get; set; } = new List<BusinessImage>();
        
        public List<Review> Reviews { get; set; } = new List<Review>();
        
        public bool IsFeatured { get; set; }
        
        public bool IsPremium { get; set; }
        
        public bool IsVerified { get; set; }
        
        public double Rating { get; set; }
        
        public int ReviewsCount { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
