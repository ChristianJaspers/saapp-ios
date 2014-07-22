using System;
using System.Linq;
using System.Collections.Generic;
using BetterSalesman.Core.BusinessLayer;
using Newtonsoft.Json;
using BetterSalesman.Core.DataLayer;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public class ServiceProviderArgument : BaseServiceProvider
    {
        private static ServiceProviderArgument instance;
        private static object locker = new Object();
        
        // Paths
//        const string pathAuth = "arguments";
        const string pathArguments = "api/json/get/bVEDGtpQmW?indent=2";
        
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
        
        #region Requests
        
        public async void Arguments(
            HttpRequestSuccessEventHandler success = null, 
            HttpRequestFailureEventHandler failure = null
        )
        {
            var request = new HttpRequest {
                Method = HTTPMethod.GET,
                Path = pathArguments,
                Parameters = ParametersWithDeviceInfo(new Dictionary<string, object>())
            };
            
            request.Success += result => {
                
                var responseJsonArguments = JsonConvert.DeserializeObject<ResponseJsonArguments>(result);
                
                DatabaseHelper.ReplaceAll<Argument>(responseJsonArguments.Arguments);
                
                if ( success != null )
                {
                    success(result);
                }
            };
            
            request.Failure += failure;
            
            await request.Perform();
        }
        
        
        #endregion
        
        #region Json definitions
        
        class ResponseJsonArguments
        {
            [JsonPropertyAttribute(PropertyName = "arguments")]
            public List<Argument> Arguments;
        }
        
        #endregion
    }
}

