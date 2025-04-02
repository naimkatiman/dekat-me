using System.Text.Json.Serialization;

namespace DekatMe.Core.Entities
{
    public class BusinessHour
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string BusinessId { get; set; } = string.Empty;
        
        [JsonIgnore]
        public Business? Business { get; set; }
        
        public int DayOfWeek { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public bool IsClosed { get; set; }
        
        public string DayName => GetDayName(DayOfWeek);
        
        public static string GetDayName(int dayOfWeek)
        {
            return dayOfWeek switch
            {
                0 => "Sunday",
                1 => "Monday",
                2 => "Tuesday",
                3 => "Wednesday",
                4 => "Thursday", 
                5 => "Friday",
                6 => "Saturday",
                _ => "Unknown"
            };
        }
        
        public string GetFormattedHours()
        {
            if (IsClosed)
                return "Closed";
                
            if (OpenTime == null || CloseTime == null)
                return "Hours not specified";
                
            return $"{OpenTime?.ToString(@"hh\:mm tt")} - {CloseTime?.ToString(@"hh\:mm tt")}";
        }
        
        public static List<BusinessHour> GetDefaultHours(string businessId)
        {
            var defaultOpenTime = new TimeSpan(9, 0, 0); // 9:00 AM
            var defaultCloseTime = new TimeSpan(17, 0, 0); // 5:00 PM
            
            var hours = new List<BusinessHour>();
            
            // Create hours for each day of the week
            for (int i = 0; i < 7; i++)
            {
                var isWeekend = i == 0 || i == 6; // Sunday or Saturday
                
                hours.Add(new BusinessHour
                {
                    BusinessId = businessId,
                    DayOfWeek = i,
                    OpenTime = isWeekend ? null : defaultOpenTime,
                    CloseTime = isWeekend ? null : defaultCloseTime,
                    IsClosed = isWeekend
                });
            }
            
            return hours;
        }
    }
}
