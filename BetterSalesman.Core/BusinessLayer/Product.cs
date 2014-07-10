using System;
using BetterSalesman.Core.BusinessLayer.Contracts;

namespace BetterSalesman.Core.BusinessLayer
{
    public class Product : IBusinessEntity
    {
        [PrimaryKey]
        [JsonPropertyAttribute(PropertyName = "id")]
        public int Id { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "name")]
        public int Name { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}

