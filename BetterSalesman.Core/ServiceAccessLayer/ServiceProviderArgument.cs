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
        const string paramRateValue = "rating";
        
        // Paths
        const string pathRate = "api/v1/arguments/{0}/rating";
        
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
            Action<Argument> success = null, 
            Action<int> failure = null
        )
        {
            var parameters = new Dictionary<string, object> {
                {paramRateValue,rating}
            };

            var request = new HttpRequest <ResponseJsonArgumentRating> {
                Method = HTTPMethod.POST,
                Path = string.Format(pathRate,argument.Id),
                Parameters = ParametersWithDeviceInfo(parameters),
                Success = response => {

                    DatabaseHelper.Replace<Argument>(response.MappedResponse.Argument);

                    if ( success != null )
                    {
                        success(response.MappedResponse.Argument);
                        
                        
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

