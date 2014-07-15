using System;
using System.Collections.Generic;
using BetterSalesman.Core.BusinessLayer;
using Newtonsoft.Json;

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
        const string pathAuth = "auth/login";
        
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
            HTTPRequestFailureEventHandler failure = null
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
            
            request.Success += result => {
                
                var resultObject = JsonConvert.DeserializeObject<JsonLoginResponse>(result);
                
                // session save
                UserSessionManager.Instance.User = new UserSession {
                    Token = resultObject.AccessToken,
                };

                UserSessionManager.Instance.Save();
                
                // db save
                // TODO save user object in DB here
                
                if ( success != null )
                {
                    success(result);
                }
            };
            
            request.Failure += failure;
            
            await request.Perform();
        }
        
        class JsonLoginResponse
        {
            [JsonPropertyAttribute(PropertyName = "user")]
            public User User;

            [JsonPropertyAttribute(PropertyName = "access_token")]
            public string AccessToken;
        }
    }
}

