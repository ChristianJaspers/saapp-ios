using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public class ServiceProviderUser
    {
        private static ServiceProviderUser instance;
        private static object locker = new Object();
        
        // Parameters
        const string paramUsername = "username";
        const string paramPassword = "password";
        const string paramDeviceInfo = "device_info";
        const string paramCurrentPlatform = "ios";
        
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
                Parameters = ParametersWithDeviceInfo(parameters)
            };
            
            request.Success += success;
            request.Failure += failure;
            request.Timeout += timeout;
            
            await request.PerformRequest(request);
        }

        Dictionary<string, object> ParametersWithDeviceInfo(Dictionary<string, object> parameters)
        {
            parameters.Add(paramDeviceInfo, new Device {
                NotificationToken = "notification token goes here", // TODO
                Platform = paramCurrentPlatform,
                Language = HttpConfig.Lang
            });
            
            return parameters;
        }
        
        class Device
        {
            public string NotificationToken;
            public string Platform;
            public string Language;
        }
    }
}

