using System.Collections.Generic;
using Newtonsoft.Json;
using BetterSalesman.Core.BusinessLayer;

namespace BetterSalesman.Core.ServiceAccessLayer.DataTransferObject
{
    public class T
    {
        [JsonPropertyAttribute(PropertyName = "users")]
        public List<User> Users;

        [JsonPropertyAttribute(PropertyName = "product_groups")]
        public List<ProductGroup> ProductGroups;

        [JsonPropertyAttribute(PropertyName = "arguments")]
        public List<Argument> Arguments;
    }
}

