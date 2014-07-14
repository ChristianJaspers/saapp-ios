using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public enum HTTPMethod
    {
        GET = 0,  
        POST = 1, 
        PUT = 2,  
        DELETE = 3
    };
    
    public class HttpRequestSetup
    {
        public string Path;
        public HTTPMethod Method;
        public Dictionary<string, string> Parameters;
        public Dictionary<string, string> Headers;
    }
    
    public class BaseHttpClient
    {
        private static BaseHttpClient instance;
        private HttpClient client;
        private static object locker = new Object();

        private string BaseUrl
        {
            get { return client.BaseAddress.ToString(); }   
        }
        
        public static BaseHttpClient Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new BaseHttpClient();
                        }
                    }
                }

                return instance;
            }
        }

        private BaseHttpClient()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://" + HttpConfig.Host);
        }
        
        public async Task<string> PerformRequest(HttpRequestSetup requestSetup)
        {
            return await PerformRequest(requestSetup.Method, requestSetup.Path, JsonConvert.SerializeObject(requestSetup.Parameters), JsonConvert.SerializeObject(requestSetup.Headers));
        }
        
        private async Task<string> PerformRequest(HTTPMethod method, string resourcePath, string serializedParameters = null, string serializedHeaders = null)
        {
            string result = null;
            
            Debug.WriteLine("HTTPClient Method: " + method);
            Debug.WriteLine("HTTPClient Path: " + BaseUrl + resourcePath);
            Debug.WriteLine("HTTPClient Params: " + serializedParameters);
            Debug.WriteLine("HTTPClient Headers: " + serializedHeaders);
            
            switch(method)
            {
                case HTTPMethod.GET:
                    result = await PerformGetRequest(resourcePath, serializedParameters);
                break;
            
                case HTTPMethod.POST:
                    result = await PerformPostRequest(resourcePath, serializedParameters);
                break;
                
                case HTTPMethod.PUT:
                    result = await PerformPutRequest(resourcePath, serializedParameters);
                break;
                
                case HTTPMethod.DELETE:
                    result = await PerformDeleteRequest(resourcePath);
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
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception while reading response [{0}]: {1}", resourcePath, e);
                
                throw e;
            }

            return await response.Content.ReadAsStringAsync();
        }
    }
}