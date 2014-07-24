using System;
using SQLite;
using Newtonsoft.Json;
using BetterSalesman.Core.BusinessLayer.Contracts;

namespace BetterSalesman.Core.BusinessLayer
{
    public class ProductGroup : IBusinessEntity
	{
        [PrimaryKey]
        [JsonPropertyAttribute(PropertyName = "id")]
        public int Id { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "position")]
        public string Position { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "title")]
        public string Title { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "archived_at")]
        public DateTime ArchivedAt { get; set; }
	}

}

