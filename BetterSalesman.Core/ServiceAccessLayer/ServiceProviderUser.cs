using System;
using System.Collections.Generic;
using BetterSalesman.Core.BusinessLayer;
using Newtonsoft.Json;
using BetterSalesman.Core.DataLayer;
using System.Threading.Tasks;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public class ServiceProviderUser : BaseServiceProvider
    {
        private static ServiceProviderUser instance;
        private static object locker = new Object();
        
        // Parameters
        const string paramEmail = "email";
        const string paramPassword = "password";
        
        // Paths
        const string pathAuth = "api/v1/auth";
        const string pathProfile = "api/v1/profile";
        const string pathForgotPassword = "api/v1/passwords";
        
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
        
        #region Requests
        
        public async void Authentication(
            string email, 
            string password, 
            HttpRequestSuccessEventHandler success = null, 
            HttpRequestFailureEventHandler failure = null
        )
        {
            var parameters = new Dictionary<string, object> {
                {paramEmail,email},
                {paramPassword,password}
            };
            
            var request = new HttpRequest {
                Method = HTTPMethod.POST,
                Path = pathAuth,
                Parameters = ParametersWithDeviceInfo(parameters)
            };
            
            request.Success += result => {
                
                var responseJsonLogin = JsonConvert.DeserializeObject<ResponseJsonLogin>(result);
                
                UserSessionManager.Instance.User = new UserSession {
                    UserId = responseJsonLogin.User.Id,
                    Token = responseJsonLogin.AccessToken,
                };

                UserSessionManager.Instance.Save();
                
                DatabaseHelper.Replace<User>(responseJsonLogin.User);
                
                if ( success != null )
                {
                    success(result);
                }
            };
            
            request.Failure += failure;
            
            await request.Perform();
        }
        
        public async void Profile(
            HttpRequestSuccessEventHandler success = null, 
            HttpRequestFailureEventHandler failure = null
        )
        {
            var request = new HttpRequest {
                Method = HTTPMethod.GET,
                Path = pathProfile,
                Parameters = ParametersWithDeviceInfo(new Dictionary<string, object>())
            };

            request.Success += result => {

                var responseJsonLogin = JsonConvert.DeserializeObject<ResponseJsonLogin>(result);

                DatabaseHelper.Replace<User>(responseJsonLogin.User);

                if ( success != null )
                {
                    success(result);
                }
            };

            request.Failure += failure;

            await request.Perform();
        }

        public async void ForgotPassword(
            string email,
            HttpRequestSuccessEventHandler success = null, 
            HttpRequestFailureEventHandler failure = null
        )
        {
            var parameters = new Dictionary<string, object> {
                {paramEmail,email},
            };
            
            var request = new HttpRequest {
                Method = HTTPMethod.POST,
                Path = pathForgotPassword,
                Parameters = ParametersWithDeviceInfo(parameters)
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

		public async Task<FileUploadResult> UpdateAvatar(string localFilePath, string mimeType)
		{
			var uploader = new HttpClientFileUploader(HttpRequest.AuthorizationToken);

			var uploadUrl = HttpConfig.ApiBaseAddress + "profile/avatar";
			var parameterName = "file";

			var result = await uploader.UploadFileAsync(uploadUrl, localFilePath, parameterName, mimeType, HttpClientFileUploader.HttpMethodPut);
			if (result.IsSuccess)
			{
				DatabaseHelper.Replace<User>(result.User);
			}

			return result;
		}
        
        #endregion
        
        #region Json definitions
        
        class ResponseJsonLogin
        {
            [JsonPropertyAttribute(PropertyName = "user")]
            public User User;

            [JsonPropertyAttribute(PropertyName = "access_token")]
            public string AccessToken;
        }
        
        
        class ResponseJsonProfile
        {
            [JsonPropertyAttribute(PropertyName = "user")]
            public User User;
        }
        
        #endregion
    }
}

