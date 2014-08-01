using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BetterSalesman.Core.DataLayer;
using BetterSalesman.Core.ServiceAccessLayer.DataTransferObject;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    internal class SynchronizationManagerApplication : SynchronizationManagerBase
    {
        #region Singleton

        private static SynchronizationManagerApplication instance;

        public static SynchronizationManagerApplication Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new SynchronizationManagerApplication();
                        }
                    }
                }

                return instance;
            }
        }

        #endregion Singleton

        private SynchronizationManagerApplication()
            : base()
        {
        }

        public override void FullSynchronizationTaskRun()
        {
            ServiceProviderSynchronization.Instance.Synchronize(
                async result => {
                
                    await UpdateDatabaseAsync(result);
                
                    OnUpdatedDatabase();
                
                    OnFinishedSynchronization();
                },
                // TODO display error message
                errorMessage => OnFinishedSynchronization()
            );
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

        private async Task UpdateDatabaseAsync(T dataContainer)
        {
            await Task.Run(() =>
                {
                    DatabaseProvider.FullSync(dataContainer);
                });
        }
    }
}