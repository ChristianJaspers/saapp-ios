﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Text;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public enum HTTPMethod
    {
        GET,
        POST,
        PUT,
        DELETE
    };
    
    public delegate void HttpRequestSuccessEventHandler(string result);
    public delegate void HttpRequestFailureEventHandler(int errorCode);
    
    public class HttpRequest
    {
        public string Path;
        public HTTPMethod Method;
        public Dictionary<string, object> Parameters;
        public Dictionary<string, string> Headers;
        
        public string AuthorizationToken;
        
        private HttpClient client;

        public event HttpRequestSuccessEventHandler Success;
        public event HttpRequestFailureEventHandler Failure;

        public async Task<string> Perform()
        {
            string result = null;
            
            client = new HttpClient();
            client.BaseAddress = new Uri("http://" + HttpConfig.Host);
            
            if (AuthorizationToken != null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(AuthorizationToken);
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
            }

            return result;
        }

        private async Task<string> PerformRequestGet(string resourcePath, string serializedParameters)
        {
            string result = null;

            try
            {
                using (HttpResponseMessage response = await client.GetAsync(resourcePath))
                {
                    result = await ParseResponse(response, resourcePath);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);   
            }

            return result;
        }

        private async Task<string> PerformRequestPost(string resourcePath, string serializedParameters)
        {
            string result = null;

            var content = new StringContent(serializedParameters, Encoding.UTF8, "application/json");
            
            try
            {
                using (HttpResponseMessage response = await client.PostAsync(resourcePath, content))
                {   
                    result = await ParseResponse(response, resourcePath);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }


            return result;
        }

        private async Task<string> PerformRequestPut(string resourcePath, string serializedParameters)
        {
            string result = null;

            var content = new StringContent(serializedParameters, Encoding.UTF8, "application/json");

            try
            {
                using (HttpResponseMessage response = await client.PutAsync(resourcePath, content))
                {
                    result = await ParseResponse(response, resourcePath);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);   
            }

            return result;
        }

        private async Task<string> PerformRequestDelete(string resourcePath)
        {
            string result = null;

            try
            {
                using (HttpResponseMessage response = await client.DeleteAsync(resourcePath))
                {
                    result = await ParseResponse(response, resourcePath);
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
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
    
                default:
                    if (Failure != null)
                    {
                        // TODO handle error here
                        Failure(Convert.ToInt32(response.StatusCode));
                    }
                    break;
            }

            return result;
        }

        void HandleException(Exception ex)
        {
            Debug.WriteLine("Error communicating with the server: " + ex.Message);
            
            Failure(Convert.ToInt32(HttpStatusCode.RequestTimeout));
        }
    }
}

