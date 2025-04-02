using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DekatMe.Api.Models
{
    public class BusinessImage
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string BusinessId { get; set; } = string.Empty;
        
        [ForeignKey("BusinessId")]
        public Business? Business { get; set; }
        
        [Required]
        public string Url { get; set; } = string.Empty;
        
        public string Alt { get; set; } = string.Empty;
        
        public bool IsPrimary { get; set; }
        
        public int DisplayOrder { get; set; }
    }
}
