using System;
using BetterSalesman.Core.BusinessLayer.Contracts;
using SQLite;
using Newtonsoft.Json;

namespace BetterSalesman.Core.BusinessLayer
{
    public class User : IBusinessEntity
    {
        [PrimaryKey]
        [JsonPropertyAttribute(PropertyName = "id")]
        public int Id { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "display_name")]
        public string DisplayName { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "email")]
        public string Email { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "image_url")]
        public string ImageUrl { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "thumb_url")]
        public string ThumbUrl { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}