using System;
using System.Collections.Generic;
using BetterSalesman.Core.BusinessLayer;
using Newtonsoft.Json;
using BetterSalesman.Core.DataLayer;

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
//        const string pathAuth = "auth/login";
        const string pathAuth = "api/json/get/cgefSlWzeG?indent=2";
        
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
            string email, 
            string password, 
            HttpRequestSuccessEventHandler success = null, 
            HttpRequestFailureEventHandler failure = null
        )
        {
            var parameters = new Dictionary<string, object> {
                {paramUsername,email},
                {paramPassword,password}
            };
            
            var request = new HttpRequest {
                Method = HTTPMethod.POST,
                Path = pathAuth,
                Parameters = ParametersWithDeviceInfo(parameters)
            };
            
            request.Success += result => {
                
                var responseJsonLogin = JsonConvert.DeserializeObject<ResponseJsonLogin>(result);
                
                // session save
                UserSessionManager.Instance.User = new UserSession {
                    Token = responseJsonLogin.AccessToken,
                };

                UserSessionManager.Instance.Save();
                
                // db save
                DatabaseHelper.Replace<User>(responseJsonLogin.User);
                
                if ( success != null )
                {
                    success(result);
                }
            };
            
            request.Failure += failure;
            
            await request.Perform();
        }
        
        class ResponseJsonLogin
        {
            [JsonPropertyAttribute(PropertyName = "user")]
            public User User;

            [JsonPropertyAttribute(PropertyName = "access_token")]
            public string AccessToken;
        }
    }
}

