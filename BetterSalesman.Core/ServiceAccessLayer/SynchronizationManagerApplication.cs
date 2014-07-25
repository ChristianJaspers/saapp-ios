using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BetterSalesman.Core.DataLayer;
using BetterSalesman.Core.BusinessLayer;
using BetterSalesman.Core.ServiceAccessLayer.DataTransferObject;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    internal class AppSynchronizationManager : SynchronizationManagerBase
    {
        #region Singleton

        private static AppSynchronizationManager instance;

        public static AppSynchronizationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new AppSynchronizationManager();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion Singleton

        private AppSynchronizationManager()
            : base()
        {
        }

        public override void FullSynchronizationTaskRun()
        {
            Task.Run(async () =>
                {
                    ServiceProviderSynchronization.Instance.Synchronize(
                        async result => {
                        
                            var dataContainer = await ParseJsonAsync(result);  
                        
                            await UpdateDatabaseAsync(dataContainer);
                        
                            OnUpdatedDatabase();
                        
                            OnFinishedSynchronization();
                        },
                        errorCode => OnFinishedSynchronization()
                    );
                });
        }

        public async Task CopyInitialDatabaseAsync()
        {
            await Task.Run(() =>
                {
                    Debug.WriteLine("Copying initial databse...");
                    DatabaseProvider.Setup();
                    Debug.WriteLine("Finished copying initial databse.");
                });
        }

        private async Task<JsonSynchronization> ParseJsonAsync(string eventJson)
        {
            Debug.WriteLine("Started JSON deserialization");
            JsonSynchronization result = await Task.Run<JsonSynchronization>(() =>
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

                    JsonSynchronization eventObject = JsonConvert.DeserializeObject<JsonSynchronization>(eventJson, jsonSerializationSettings);

                    if (errors.Any())
                    {
                        Debug.WriteLine("Error! There was an error while deserializing JSON");
                        Debug.WriteLine("Errors details: " + string.Join(", ", errors));
                        eventObject = null;
                    }

                    return eventObject;
                });
            Debug.WriteLine("Finished JSON deserialization");

            return result;
        }

        private async Task UpdateDatabaseAsync(JsonSynchronization dataContainer)
        {
            await Task.Run(() =>
                {
                    DatabaseProvider.FullSync(dataContainer);
                });
        }
    }
}