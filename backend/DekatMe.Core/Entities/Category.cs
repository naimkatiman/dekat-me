using System.Text.Json.Serialization;

namespace DekatMe.Core.Entities
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public int Count { get; set; }
        
        [JsonIgnore]
        public List<Business> Businesses { get; set; } = new List<Business>();
        
        public string GetIconDisplay()
        {
            return !string.IsNullOrEmpty(Icon) ? Icon : "📁";
        }
        
        public string GetCategoryUrl()
        {
            return $"/category/{Slug}";
        }
    }
}
