using System.Collections.Generic;
using Newtonsoft.Json;
using BetterSalesman.Core.BusinessLayer;

namespace BetterSalesman.Core.ServiceAccessLayer.DataTransferObject
{
    public class T
    {
        [JsonPropertyAttribute(PropertyName = "users")]
        public List<User> Users;

        [JsonPropertyAttribute(PropertyName = "categories")]
        public List<ProductGroup> Categories;

        [JsonPropertyAttribute(PropertyName = "arguments")]
        public List<Argument> Arguments;

        [JsonPropertyAttribute(PropertyName = "reports")]
        public List<Report> Reports;
    }
}

