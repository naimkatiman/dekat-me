using System.ComponentModel.DataAnnotations;

namespace DekatMe.Api.Models
{
    public class Category
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(50)]
        public string Slug { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
        
        [MaxLength(10)]
        public string Icon { get; set; } = string.Empty;
        
        public int Count { get; set; }
        
        public List<Business> Businesses { get; set; } = new List<Business>();
    }
}
