using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace BetterSalesman.Core.ServiceAccessLayer
{
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
        

        private async Task<string> PerformGetRequest(string resourcePath)
        {
            Debug.WriteLine("HTTPClient: Performing get request for path: " + BaseUrl + resourcePath);

            string result = null;
            
            using (HttpResponseMessage response = await client.GetAsync(resourcePath))
            {
                try
                {
                    response.EnsureSuccessStatusCode();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Exception while reading response [{0}]: {1}", resourcePath, e);
                }

                result = await response.Content.ReadAsStringAsync();
            }

            return result;
        }

        private async Task<string> PerformPostRequest(string resourcePath, string serializedParameters)
        {
            Debug.WriteLine("HTTPClient: Performing post request for path: " + BaseUrl + resourcePath);

            string result = null;

            var content = new StringContent(serializedParameters);

            Debug.WriteLine("Sending content via POST: " + serializedParameters);

            using (HttpResponseMessage response = await client.PostAsync(resourcePath, content))
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

                result = await response.Content.ReadAsStringAsync();
            }

            return result;
        }
    }
}