using System;
using System.Collections.Generic;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public class ServiceProviderUser : BaseServiceProvider
    {
        private static ServiceProviderUser instance;
        private static object locker = new Object();
        
        // Parameters
        const string paramUsername = "username";
        const string paramPassword = "password";
        
        // Paths
        const string pathAuth = "auth.json";
        
        public static ServiceProviderUser Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new ServiceProviderUser();
                        }
                    }
                }

                return instance;
            }
        }
        
        public async void Authentication(
            string username, 
            string password, 
            HTTPRequestSuccessEventHandler success = null, 
            HTTPRequestFailureEventHandler failure = null, 
            HTTPRequestTimeoutEventHandler timeout = null
        )
        {
            var parameters = new Dictionary<string, object> {
                {paramUsername,username},
                {paramPassword,password}
            };
            
            var request = new HttpRequest {
                Method = HTTPMethod.POST,
                Path = pathAuth,
                Parameters = ParametersWithDeviceInfo(parameters),
                AuthorizationToken = "testtoken"
            };
            
            request.Success += success;
            request.Failure += failure;
            request.Timeout += timeout;
            
            await request.PerformRequest(request);
        }
    }
}

