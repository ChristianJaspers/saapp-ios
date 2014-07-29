using System;
using System.Collections.Generic;
using BetterSalesman.Core.BusinessLayer;
using Newtonsoft.Json;
using BetterSalesman.Core.DataLayer;
using System.Threading.Tasks;
using BetterSalesman.Core.ServiceAccessLayer.DataTransferObject;


using BetterSalesman.Core.ServiceAccessLayer.DataTransferObject;

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
            Action<string> success = null, 
            Action<int> failure = null
        )
        {
            var parameters = new Dictionary<string, object> {
                {paramEmail,email},
                {paramPassword,password}
            };
            
            var request = new HttpRequest <ResponseJsonLogin> {
                Method = HTTPMethod.POST,
                Path = pathAuth,
                Parameters = ParametersWithDeviceInfo(parameters),
                Success = response => {
                    UserSessionManager.Instance.User = new UserSession {
                        UserId = response.MappedResponse.User.Id,
                        Token = response.MappedResponse.AccessToken,
                    };

                    UserSessionManager.Instance.Save();

                    DatabaseHelper.Replace<User>(response.MappedResponse.User);
                    
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

        public async void ForgotPassword(
            string email,
            Action<string> success = null, 
            Action<int> failure = null
        )
        {
            var parameters = new Dictionary<string, object> {
                {paramEmail,email},
            };
            
            var request = new HttpRequest <JsonEmpty> {
                Method = HTTPMethod.POST,
                Path = pathForgotPassword,
                Parameters = ParametersWithDeviceInfo(parameters),
                Success = response => {
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

		public async Task<FileUploadResult> UpdateAvatar(string localFilePath, string mimeType)
		{   
            var uploader = new HttpClientFileUploader(UserSessionManager.Instance.AccessToken);

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

