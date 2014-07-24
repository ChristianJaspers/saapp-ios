using System;
using BetterSalesman.Core.BusinessLayer.Contracts;
using SQLite;
using Newtonsoft.Json;

namespace BetterSalesman.Core.BusinessLayer
{
    public class Report : IBusinessEntity
	{
        [PrimaryKey]
        [JsonPropertyAttribute(PropertyName = "id")]
        public int Id { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "title")]
        public string Title { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "level")]
        public int Level { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }
    }

}
