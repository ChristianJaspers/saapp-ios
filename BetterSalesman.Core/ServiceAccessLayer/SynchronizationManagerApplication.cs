using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BetterSalesman.Core.DataLayer;
using BetterSalesman.Core.ServiceAccessLayer.DataTransferObject;
using System;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public class SynchronizationManagerApplication : SynchronizationManagerBase
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

		public void ExecuteActionAsSoonAsSynchronizationNotRunning(Action action)
		{
			if (IsSynchronizationInProgress)
			{
				// TODO - execute action on sync finished
			}
			else
			{
				// TODO - execute action immediately
			}
		}

		// TODO - consider passing SynchronizationResult to OnFinishedSynchronization 
		//		  (for example informing whether it was successful or not and any Error objects that occured during the process)
        public override void FullSynchronizationTaskRun()
        {
            ServiceProviderSynchronization.Instance.Synchronize(
                async result => {
                
                    await UpdateDatabaseAsync(result);
                
                    OnUpdatedDatabase();
                
                    OnFinishedSynchronization();
                },
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