using System;
using SQLite;
using Newtonsoft.Json;
using BetterSalesman.Core.BusinessLayer.Contracts;

namespace BetterSalesman.Core.BusinessLayer
{
    public class Category : IBusinessEntity
	{
        [PrimaryKey]
        [JsonPropertyAttribute(PropertyName = "id")]
        public int Id { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "position")]
        public string Position { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "name")]
        public string Name { get; set; }
	}

}

