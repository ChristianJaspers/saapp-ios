using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public enum HTTPMethod
    {
        GET,
        POST,
        PUT,
        DELETE
    };
    
    public delegate void HTTPRequestSuccessEventHandler(string result);
    public delegate void HTTPRequestFailureEventHandler(int errorCode);
    public delegate void HTTPRequestTimeoutEventHandler();
    
    public class HttpRequest
    {
        public string Path;
        public HTTPMethod Method;
        public Dictionary<string, object> Parameters;
        public Dictionary<string, string> Headers;
        
        private HttpClient client;

        public event HTTPRequestSuccessEventHandler Success;
        public event HTTPRequestFailureEventHandler Failure;
        public event HTTPRequestTimeoutEventHandler Timeout;

        public async Task<string> PerformRequest(HttpRequest requestSetup)
        {
            string result = null;
            
            client = new HttpClient();
            client.BaseAddress = new Uri("http://" + HttpConfig.Host);

            string serializedParameters = JsonConvert.SerializeObject(requestSetup.Parameters);
            string serializedHeaders = JsonConvert.SerializeObject(requestSetup.Headers);
            string path = requestSetup.Path;

            Debug.WriteLine("HTTPClient Method: " + requestSetup.Method);
            Debug.WriteLine("HTTPClient Path: " + client.BaseAddress + requestSetup.Path);
            Debug.WriteLine("HTTPClient Params: " + serializedParameters);
            Debug.WriteLine("HTTPClient Headers: " + serializedHeaders);

            switch(requestSetup.Method)
            {
            case HTTPMethod.GET:
                result = await PerformGetRequest(path, serializedParameters);
                break;

            case HTTPMethod.POST:
                result = await PerformPostRequest(path, serializedParameters);
                break;

            case HTTPMethod.PUT:
                result = await PerformPutRequest(path, serializedParameters);
                break;

            case HTTPMethod.DELETE:
                result = await PerformDeleteRequest(path);
                break;
            }

            return result;
        }

        private async Task<string> PerformGetRequest(string resourcePath, string serializedParameters)
        {
            string result = null;

            using (HttpResponseMessage response = await client.GetAsync(resourcePath))
            {
                result = await ParseResponse(response, resourcePath);
            }

            return result;
        }

        private async Task<string> PerformPostRequest(string resourcePath, string serializedParameters)
        {
            string result = null;

            var content = new StringContent(serializedParameters);

            using (HttpResponseMessage response = await client.PostAsync(resourcePath, content))
            {
                result = await ParseResponse(response, resourcePath);
            }

            return result;
        }

        private async Task<string> PerformPutRequest(string resourcePath, string serializedParameters)
        {
            string result = null;

            var content = new StringContent(serializedParameters);

            using (HttpResponseMessage response = await client.PutAsync(resourcePath, content))
            {
                result = await ParseResponse(response, resourcePath);
            }

            return result;
        }

        private async Task<string> PerformDeleteRequest(string resourcePath)
        {
            string result = null;

            using (HttpResponseMessage response = await client.DeleteAsync(resourcePath))
            {
                result = await ParseResponse(response, resourcePath);
            }

            return result;
        }

        async Task<String> ParseResponse(HttpResponseMessage response, string resourcePath)
        {
            Debug.WriteLine("HTTPClient Result Status code: " + response.StatusCode);

            string result = string.Empty;

            switch(response.StatusCode)
            {
                case HttpStatusCode.OK:
                    result = await response.Content.ReadAsStringAsync();
    
                    Debug.WriteLine("HTTPClient Result content: " + result);
    
                    if (Success != null)
                    {
                        Success(result);
                    }
                    break;
    
                case HttpStatusCode.RequestTimeout:
                    if (Timeout != null)
                    {
                        // TODO handle error here
                        Timeout();
                    }
                    break;
    
                default:
                    if (Failure != null)
                    {
                        // TODO handle error here
                        Failure(400);
                    }
                    break;
            }

            return result;
        }
    }
}

