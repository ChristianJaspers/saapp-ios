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

        [JsonPropertyAttribute(PropertyName = "product_group_id")]
        public int ProductGroupId { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "user_id")]
        public int UserId { get; set; }

        [JsonPropertyAttribute(PropertyName = "feature")]
        public string Feature { get; set; }

        [JsonPropertyAttribute(PropertyName = "benefit")]
        public string Benefit { get; set; }

        [JsonPropertyAttribute(PropertyName = "rating")]
        public int Rating { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "my_rating")]
        public int MyRating { get; set; }

        [JsonPropertyAttribute(PropertyName = "created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonPropertyAttribute(PropertyName = "updated_at")]
        public DateTime UpdatedAt { get; set; }
        
        #region Convenience properties
        
        public bool Rated
        {
            get { return Rating > 0; }   
        }
        
        #endregion
    }
}

