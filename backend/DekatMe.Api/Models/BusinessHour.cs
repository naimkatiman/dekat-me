using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DekatMe.Api.Models
{
    public class BusinessHour
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        
        [Required]
        public string BusinessId { get; set; } = string.Empty;
        
        [ForeignKey("BusinessId")]
        public Business? Business { get; set; }
        
        [Required]
        [Range(0, 6)]
        public int DayOfWeek { get; set; }
        
        public TimeSpan? OpenTime { get; set; }
        
        public TimeSpan? CloseTime { get; set; }
        
        public bool IsClosed { get; set; }
    }
}
