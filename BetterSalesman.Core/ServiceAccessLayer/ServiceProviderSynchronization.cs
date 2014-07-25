using System;
using System.Collections.Generic;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public class ServiceProviderSynchronization : BaseServiceProvider
    {
        private static ServiceProviderSynchronization instance;
        private static object locker = new Object();
        
        // Paths
        const string pathSynchronization = "api/v1/resources";
        
        public static ServiceProviderSynchronization Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new ServiceProviderSynchronization();
                        }
                    }
                }

                return instance;
            }
        }
        
        #region Requests
        
        public async void Synchronize(
            HttpRequestSuccessEventHandler success = null, 
            HttpRequestFailureEventHandler failure = null
        )
        {
            var request = new HttpRequest {
                Method = HTTPMethod.GET,
                Path = pathSynchronization,
                Parameters = ParametersWithDeviceInfo(new Dictionary<string, object>())
            };
            
            request.Success += result => {

                if ( success != null )
                {
                    success(result);
                }
            };

            request.Failure += failure;
            
            await request.Perform();
        }
        
        #endregion
    }
}

