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
        
        [JsonPropertyAttribute(PropertyName = "avatar_url")]
        public string AvatarUrl { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "avatar_thumb_url")]
        public string AvatarThumbUrl { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "experience")]
        public int Experience { get; set; }

		[JsonPropertyAttribute(PropertyName = "my_activity")]
		public float MyActivity { get; set; }

		[JsonPropertyAttribute(PropertyName = "my_team_activity")]
		public float MyTeamActivity { get; set; }

		[JsonPropertyAttribute(PropertyName = "all_teams_activity")]
		public float AllTeamsActivity { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}