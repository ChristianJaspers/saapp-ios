using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BetterSalesman.Core.DataLayer;
using BetterSalesman.Core.ServiceAccessLayer.DataTransferObject;
using System;
using System.Timers;

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

		#region Background timer

		/// <summary>
		/// The synchronization in background invocation interval in milliseconds
		/// </summary>
		const double SynchronizationInBackgroundInvocationInterval = 30 * 60 * 1000;
		Timer synchronizationInBackgroundInvocationTimer;

		#endregion Background timer

		#region SynchronizationCancelledLocker

		private object synchronizationCancelledLocker = new object();
		private bool shouldCancelSynchronization;

		public bool ShouldCancelSynchronization
		{
			get 
			{
				lock (synchronizationCancelledLocker)
				{
					return shouldCancelSynchronization;
				}
			}

			private set
			{
				lock (synchronizationCancelledLocker)
				{
					shouldCancelSynchronization = value;
				}
			}
		}

		#endregion SynchronizationCancelledLocker

        private SynchronizationManagerApplication()
            : base()
        {
			ShouldCancelSynchronization = false;

			synchronizationInBackgroundInvocationTimer = new Timer();
			synchronizationInBackgroundInvocationTimer.Interval = SynchronizationInBackgroundInvocationInterval;
			synchronizationInBackgroundInvocationTimer.Elapsed += (object source, ElapsedEventArgs e) => 
				{
					FullSynchronizationTaskRun();
				};
        }

		public void StartSynchronizationInBackgroundTimer()
		{
			Debug.WriteLine("INFO: Starting sync background timer");
			synchronizationInBackgroundInvocationTimer.Enabled = true;
		}

		public void StopSynchronizationInBackgroundTimer()
		{
			Debug.WriteLine("INFO: Stopping sync background timer");
			synchronizationInBackgroundInvocationTimer.Enabled = false;
		}

		/// <summary>
		/// This method is meant to be used to cancel writing to database after synchronization. Since synchronization is run in background and user can concurrently perform actions that may result in writing to database, user changes should be favored over those of synchronization (since it might have downloaded data before user updated it).
		/// For this reason this method should be used to inform the synchronization manager that it should not save the synchronization response when synchronization is finished (usually because there are other write actions to be performed that have higher priority).
		/// </summary>
		public void CancelWriteToDatabaseIfSynchronizationInProgress()
		{
			if (IsSynchronizationInProgress)
			{
				ShouldCancelSynchronization = true;
			}
		}

		// TODO - consider passing SynchronizationResult to OnFinishedSynchronization
		//		  (for example informing whether it was successful or not and any Error objects that occured during the process)
        public override void FullSynchronizationTaskRun()
        {
            ServiceProviderSynchronization.Instance.Synchronize(
                async result => 
				{
					if (result == null)
					{
						ShouldCancelSynchronization = false;
						OnFinishedSynchronization();
						return;
					}

					await Task.Run(() =>
					{
						if (ShouldCancelSynchronization)
						{
							ShouldCancelSynchronization = false;
							OnFinishedSynchronization();
							return;
						}

						DatabaseProvider.FullSync(result);
					});
                
                    OnFinishedSynchronization();
                },
                errorMessage => OnFinishedSynchronization()
            );
        }

        private async Task CopyInitialDatabaseAsync()
        {
            await Task.Run(() =>
            {
                Debug.WriteLine("Copying initial databse...");
                DatabaseProvider.Setup();
                Debug.WriteLine("Finished copying initial databse.");
            });
        }
    }
}