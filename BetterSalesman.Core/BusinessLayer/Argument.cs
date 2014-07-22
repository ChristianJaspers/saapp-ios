using System;
using BetterSalesman.Core.BusinessLayer.Contracts;
using SQLite;
using Newtonsoft.Json;

namespace BetterSalesman.Core.BusinessLayer
{
    public class Argument : IBusinessEntity
    {
        [PrimaryKey]
        [JsonPropertyAttribute(PropertyName = "id")]
        public int Id { get; set; }

        [JsonPropertyAttribute(PropertyName = "product_id")]
        public int ProductId { get; set; }

        [JsonPropertyAttribute(PropertyName = "feature")]
        public string Feature { get; set; }

        [JsonPropertyAttribute(PropertyName = "benefit")]
        public string Benefit { get; set; }

        [JsonPropertyAttribute(PropertyName = "relevance")]
        public int Relevance { get; set; }

        [JsonPropertyAttribute(PropertyName = "rated")]
        public bool Rated { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}

