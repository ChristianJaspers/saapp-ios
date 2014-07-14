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
        
        [JsonPropertyAttribute(PropertyName = "username")]
        public string Username { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "email")]
        public string Email { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "image_url")]
        public string ImageUrl { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "thumb_url")]
        public string ThumbUrl { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "activity_me")]
        public float ActivityMe { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "activity_team")]
        public float ActivityTeam { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "activity_all_teams")]
        public float ActivityAllTeams { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}