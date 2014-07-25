﻿using System;
using System.Collections.Generic;
using BetterSalesman.Core.BusinessLayer;
using Newtonsoft.Json;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public class ServiceProviderSynchronization : BaseServiceProvider
    {
        private static ServiceProviderSynchronization instance;
        private static object locker = new Object();
        
        // Paths
        const string pathSynchronization = "api/v1/sync";
        
        public static ServiceProviderSynchronization Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new ServiceProviderSynchronization();
                        }
                    }
                }

                return instance;
            }
        }
        
        #region Requests
        
        public async void Synchronize(
            HttpRequestSuccessEventHandler success = null, 
            HttpRequestFailureEventHandler failure = null
        )
        {
            var request = new HttpRequest {
                Method = HTTPMethod.GET,
                Path = pathSynchronization,
                Parameters = ParametersWithDeviceInfo(new Dictionary<string, object>())
            };
            
            request.Success += result => {
                
                var responseJsonSynchronization = JsonConvert.DeserializeObject<ResponseJsonSynchronization>(result);
                
                // TODO run sync method from DatabaseProvider ?

                if ( success != null )
                {
                    success(result);
                }
            };

            request.Failure += failure;
            
            await request.Perform();
        }
        
        #endregion

        #region Json definitions

        class ResponseJsonSynchronization
        {
            [JsonPropertyAttribute(PropertyName = "users")]
            public List<User> Users;
            
            [JsonPropertyAttribute(PropertyName = "product_groups")]
            public List<Category> ProductGroups;
            
            [JsonPropertyAttribute(PropertyName = "arguments")]
            public List<Argument> Arguments;
            
            [JsonPropertyAttribute(PropertyName = "reports")]
            public List<Report> Reports;
        }
        
        #endregion
    }
}

