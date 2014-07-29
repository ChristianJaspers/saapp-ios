using System;
using System.Collections.Generic;
using BetterSalesman.Core.BusinessLayer;
using BetterSalesman.Core.DataLayer;
using Newtonsoft.Json;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public class ServiceProviderArgument : BaseServiceProvider
    {
        private static ServiceProviderArgument instance;
        private static object locker = new Object();
        
        // Parameters
        const string paramArgumentId = "argument_id";
        const string paramRateValue = "rating";
        
        // Paths
        const string pathRate = "api/v1/argument/rate";
        
        public static ServiceProviderArgument Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new ServiceProviderArgument();
                        }
                    }
                }

                return instance;
            }
        }
        
        public async void Rate(
            Argument argument,
            float rating,
            Action<string> success = null, 
            Action<int> failure = null
        )
        {
            var parameters = new Dictionary<string, object> {
                {paramArgumentId,argument.Id},
                {paramRateValue,rating}
            };

            var request = new HttpRequest <ResponseJsonArgumentRating> {
                Method = HTTPMethod.POST,
                Path = pathRate,
                Parameters = ParametersWithDeviceInfo(parameters),
                Success = response => {

                    DatabaseHelper.Replace<Argument>(response.MappedResponse.Argument);

                    if ( success != null )
                    {
                        success(string.Empty);
                    }
                },
                Failure = response => {

                    if ( failure != null )
                    {
                        failure(response.Error.InternalCode);
                    }
                }
            };
            
            await request.Perform();
        }
        
        class ResponseJsonArgumentRating
        {
            [JsonPropertyAttribute(PropertyName = "argument")]
            public Argument Argument;
        }
    }
}

