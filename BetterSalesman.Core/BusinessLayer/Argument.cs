using System;
using BetterSalesman.Core.BusinessLayer.Contracts;

namespace BetterSalesman.Core.BusinessLayer
{
    public class Argument : IBusinessEntity
    {
        [PrimaryKey]
        [JsonPropertyAttribute(PropertyName = "id")]
        public int Id { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "product")]
        public int ProductId { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "feature")]
        public string Feature { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "benefit")]
        public string Benefit { get; set; }
        
        public DateTime UpdatedAt { get; set; }
    }
}

