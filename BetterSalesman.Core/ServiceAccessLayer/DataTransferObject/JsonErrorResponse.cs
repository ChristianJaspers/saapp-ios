using System;
using Newtonsoft.Json;

namespace BetterSalesman.Core.ServiceAccessLayer.DataTransferObject
{
    public class JsonErrorResponse
    {
        [JsonPropertyAttribute(PropertyName = "error")]
        public ServiceAccessError Error { get; set; }
    }
}

