using System;
using System.Collections.Generic;
using BetterSalesman.Core.BusinessLayer;
using BetterSalesman.Core.DataLayer;
using BetterSalesman.Core.Extensions;
using Newtonsoft.Json;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public class ServiceProviderArgument : BaseServiceProvider
    {
        private static ServiceProviderArgument instance;
        private static object locker = new Object();
        
        // Parameters
        const string paramRateValue = "rating";
        
        const string paramArgumentId = "argument_id";
        
        const string paramProductGroupId = "product_group_id";
        const string paramFeature = "feature";
        const string paramBenefit = "benefit";
        
        // Paths
        const string pathRate = "api/v1/arguments/{0}/ratings";
        
        const string pathArgumentCreate = "api/v1/arguments";
        const string pathArgumentUpdate = "api/v1/arguments/{0}";
        
        public static ServiceProviderArgument Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new ServiceProviderArgument();
                        }
                    }
                }

                return instance;
            }
        }
        
        public async void CreateOrUpdate(
            Argument argument,
            Action<Argument> success = null, 
            Action<string> failure = null
        )
        {   
            bool update = argument.Id > 0;
            
            var parameters = new Dictionary<string, object> {
                {paramProductGroupId,argument.ProductGroupId},
                {paramFeature,argument.Feature},
                {paramBenefit,argument.Benefit}
            };
            
            var method = HTTPMethod.POST;
            var path = string.Format(pathArgumentCreate);
            if (update)
            {
                parameters.Add(paramArgumentId, argument.Id);
                method = HTTPMethod.PUT;
                path = string.Format(pathArgumentUpdate, argument.Id);
            }
            
            var request = new HttpRequest <ResponseJsonArgument> {
                Method = method,
                Path = path,
                Parameters = ParametersWithDeviceInfo(parameters),
                Success = response => {
					SynchronizationManager.Instance.CancelWriteToDatabaseIfSynchronizationInProgress();
                    DatabaseHelper.Replace<Argument>(response.MappedResponse.Argument);
                    DatabaseHelper.Replace<User>(response.MappedResponse.User);

                    if ( success != null )
                    {
                        success(response.MappedResponse.Argument);
                    }
                },
                Failure = response => {

                    if ( failure != null )
                    {
                        failure(response.Error.LocalizedMessage);
                    }
                }
            };

            await request.Perform();
        }
        
        public async void Rate(
            Argument argument,
            float rating,
            Action<Argument> success = null, 
            Action<string> failure = null
        )
        {
            var parameters = new Dictionary<string, object> {
                {paramRateValue,rating}
            };

            var request = new HttpRequest <ResponseJsonArgumentRating> {
                Method = HTTPMethod.POST,
                Path = string.Format(pathRate,argument.Id),
                Parameters = ParametersWithDeviceInfo(parameters),
                Success = response => {
					SynchronizationManager.Instance.CancelWriteToDatabaseIfSynchronizationInProgress();
                    DatabaseHelper.Replace<Argument>(response.MappedResponse.Argument);
                    DatabaseHelper.Replace<User>(response.MappedResponse.User);

                    if ( success != null )
                    {
                        success(response.MappedResponse.Argument);
                    }
                },
                Failure = response => {

                    if ( failure != null )
                    {
                        failure(response.Error.LocalizedMessage);
                    }
                }
            };
            
            await request.Perform();
        }
        
        class ResponseJsonArgument
        {
            [JsonPropertyAttribute(PropertyName = "argument")]
            public Argument Argument;
            
            [JsonPropertyAttribute(PropertyName = "user")]
            public User User;
        }
        
        class ResponseJsonArgumentRating : ResponseJsonArgument
        {
        }
    }
}

