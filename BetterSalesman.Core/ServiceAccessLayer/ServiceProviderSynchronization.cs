﻿using System;
using System.Collections.Generic;
using BetterSalesman.Core.ServiceAccessLayer.DataTransferObject;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public class ServiceProviderSynchronization : BaseServiceProvider
    {
        private static ServiceProviderSynchronization instance;
        private static object locker = new Object();
        
        // Paths
        const string pathSynchronization = "api/v1/resources";
        
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
            Action<T> success = null, 
            Action<int> failure = null
        )
        {
            var request = new HttpRequest <T> {
                Method = HTTPMethod.GET,
                Path = pathSynchronization,
                Parameters = ParametersWithDeviceInfo(new Dictionary<string, object>()),
                Success = response => {
                    if ( success != null )
                    {
                        success(response.MappedResponse);
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
        
        #endregion
    }
}

