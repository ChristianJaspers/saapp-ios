using System.Collections.Generic;
using Newtonsoft.Json;
using BetterSalesman.Core.BusinessLayer;

namespace BetterSalesman.Core.ServiceAccessLayer.DataTransferObject
{
    public class JsonSynchronization
    {
        [JsonPropertyAttribute(PropertyName = "users")]
        public List<User> Users;

        [JsonPropertyAttribute(PropertyName = "categories")]
        public List<Category> Categories;

        [JsonPropertyAttribute(PropertyName = "arguments")]
        public List<Argument> Arguments;

        [JsonPropertyAttribute(PropertyName = "reports")]
        public List<Report> Reports;
    }
}

