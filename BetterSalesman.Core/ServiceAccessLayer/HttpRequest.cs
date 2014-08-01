using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Text;
using BetterSalesman.Core.ServiceAccessLayer.DataTransferObject;
using System.Linq;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public enum HTTPMethod
    {
        GET,
        POST,
        PUT,
        DELETE
    };
    
    public class HttpRequestResult <T>
    {
        public HttpRequestResult()
        {
        }

        public HttpRequestResult(ServiceAccessError error) : base()
        {
            Error = error;
        }

        public bool IsSuccess { get { return Error == null; } }
        public T MappedResponse { get; set; }
        public ServiceAccessError Error { get; set; }
    }
    
    public class HttpRequest <T>
    {
        public string Path;
        public HTTPMethod Method;
        public Dictionary<string, object> Parameters;
        public Dictionary<string, string> Headers;
        
        private HttpClient client;

        public Action<HttpRequestResult<T>> Success;
        public Action<HttpRequestResult<T>> Failure;
        
        const string ContentType = "application/json";

        public async Task<HttpRequestResult<T>> Perform()
        {
            HttpRequestResult<T> result;
            
            client = new HttpClient();
            client.BaseAddress = new Uri("http://" + HttpConfig.Host);
            
            if (!string.IsNullOrEmpty(UserSessionManager.Instance.AccessToken))
            {   
                client.DefaultRequestHeaders.Add("Authorization", "Token token=\"" + UserSessionManager.Instance.AccessToken + "\"");
            }

            string serializedParameters = JsonConvert.SerializeObject(Parameters);
            string serializedHeaders = JsonConvert.SerializeObject(Headers);
            string path = Path;

            Debug.WriteLine("HTTPClient Method: " + Method);
            Debug.WriteLine("HTTPClient Path: " + client.BaseAddress + Path);
            Debug.WriteLine("HTTPClient Params: " + serializedParameters);
            Debug.WriteLine("HTTPClient Headers: " + serializedHeaders);
            Debug.WriteLine("HTTPClient Auth token: " + client.DefaultRequestHeaders.Authorization);

            switch(Method)
            {
            case HTTPMethod.GET:
                result = await PerformRequestGet(path, serializedParameters);
                break;

            case HTTPMethod.POST:
                result = await PerformRequestPost(path, serializedParameters);
                break;

            case HTTPMethod.PUT:
                result = await PerformRequestPut(path, serializedParameters);
                break;

            case HTTPMethod.DELETE:
                result = await PerformRequestDelete(path);
                break;
                    
            default:
                result = await PerformRequestGet(path, serializedParameters);
                break;
                    
            }

            return result;
        }

        private async Task<HttpRequestResult<T>> PerformRequestGet(string resourcePath, string serializedParameters)
        {
            HttpRequestResult<T> result = default(HttpRequestResult<T>);

            try
            {
                using (HttpResponseMessage response = await client.GetAsync(resourcePath))
                {
                    result = await ParseResponse(response);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);   
            }

            return result;
        }

        private async Task<HttpRequestResult<T>> PerformRequestPost(string resourcePath, string serializedParameters)
        {
            HttpRequestResult<T> result = default(HttpRequestResult<T>);

            var content = new StringContent(serializedParameters, Encoding.UTF8, ContentType);
            
            try
            {
                using (HttpResponseMessage response = await client.PostAsync(resourcePath, content))
                {   
                    result = await ParseResponse(response);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }


            return result;
        }

        private async Task<HttpRequestResult<T>> PerformRequestPut(string resourcePath, string serializedParameters)
        {
            HttpRequestResult<T> result = default(HttpRequestResult<T>);

            var content = new StringContent(serializedParameters, Encoding.UTF8, ContentType);

            try
            {
                using (HttpResponseMessage response = await client.PutAsync(resourcePath, content))
                {
                    result = await ParseResponse(response);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);   
            }

            return result;
        }

        private async Task<HttpRequestResult<T>> PerformRequestDelete(string resourcePath)
        {
            HttpRequestResult<T> result = default(HttpRequestResult<T>);

            try
            {
                using (HttpResponseMessage response = await client.DeleteAsync(resourcePath))
                {
                    result = await ParseResponse(response);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return result;
        }

        async Task<HttpRequestResult<T>> ParseResponse(HttpResponseMessage response)
        {
            Debug.WriteLine("HTTPClient Result Status code: " + response.StatusCode);

            HttpRequestResult<T> result =  new HttpRequestResult<T>();
            
            int codeNumber = Convert.ToInt32(response.StatusCode);
            
            bool statusOK = (codeNumber >= 200 && codeNumber < 300);
            bool status4xx = (codeNumber >= 400 && codeNumber < 500);

            if ( statusOK || status4xx )
            {
                string resultString = await response.Content.ReadAsStringAsync();
                
                if ( statusOK )
                {
                    ResultOK(result, resultString);
                } 
                else
                {
                    ResultErrorMessage(result, resultString);
                }
            } 
            else
            {
                result = new HttpRequestResult<T>(ServiceAccessError.ErrorUnknown);   

                OnFailure(result);
            }

            return result;
        }

        void HandleException(Exception ex)
        {
            Debug.WriteLine("Error communicating with the server: " + ex.Message);
            
            Failure(new HttpRequestResult<T> (ServiceAccessError.ErrorUnknown));
        }

        async void ResultOK(HttpRequestResult<T> result, string resultString)
        {
            Debug.WriteLine("HTTPClient RAW response: " + resultString);

            var responseJson = await ParseJsonAsync<T>(resultString);

            if ( !responseJson.Equals(default(T)) )
            {
                result.MappedResponse = responseJson;

                OnSuccess(result);
            } 
            else
            {
                result.Error = ServiceAccessError.ErrorUnknown;

                OnFailure(result);
            }
        }

        async void ResultErrorMessage(HttpRequestResult<T> result, string resultString)
        {
            var errorJson = await ParseJsonAsync<JsonErrorResponse>(resultString);
            var isKnownError = errorJson != null && errorJson.Error != null;
            if (isKnownError)
            {
                result.Error = errorJson.Error;
            }
            else
            {
                result = new HttpRequestResult<T>(ServiceAccessError.ErrorUnknown);
            }   

            OnFailure(result);
        }
        
        private async Task<X> ParseJsonAsync<X>(string json)
        {
            Debug.WriteLine("Started JSON deserialization");
            X result = await Task.Run<X>(() =>
            {
                List<string> errors = new List<string>();
                JsonSerializerSettings jsonSerializationSettings = new JsonSerializerSettings
                    {
                        Error = (sender, args) =>
                            {
                                errors.Add(args.ErrorContext.Error.Message);
                                args.ErrorContext.Handled = true;
                            }
                    };

                X eventObject = JsonConvert.DeserializeObject<X>(json, jsonSerializationSettings);

                if (errors.Any())
                {
                    Debug.WriteLine("Error! There was an error while deserializing JSON");
                    Debug.WriteLine("Errors details: " + string.Join(", ", errors));
                    eventObject = default(X);
                }

                return eventObject;
            });
            Debug.WriteLine("Finished JSON deserialization");

            return result;
        }
        
        void OnFailure(HttpRequestResult<T> result)
        {
            if (Failure != null)
            {
                Failure(result);
            }
        }

        void OnSuccess(HttpRequestResult<T> result)
        {
            if (Success != null)
            {   
                Success(result);
            }
        }
    }
}

