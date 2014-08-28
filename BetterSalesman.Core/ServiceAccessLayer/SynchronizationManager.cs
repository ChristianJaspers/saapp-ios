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
	#region Delegates

	public delegate void StartedSynchronizationEventHandler();

	public delegate void FinishedSynchronizationEventHandler();

	#endregion Delegates

    public class SynchronizationManager
    {
        #region Singleton
		private static object locker = new object();

		private static SynchronizationManager instance;

        public static SynchronizationManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new SynchronizationManager();
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
		const double SynchronizationInBackgroundInvocationInterval = 0.25 * 60 * 1000; // calculated as minutes * seconds_per_minute * milliseconds_per_second
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

		#region Events

		public event StartedSynchronizationEventHandler StartedSynchronization;

		public event FinishedSynchronizationEventHandler FinishedSynchronization;

		#endregion Events

		#region SynchronizationInProgressLocker

		public bool IsSynchronizationInProgress
		{
			get
			{
				lock (isSynchronizationInProgressLocker)
				{
					return isSynchronizationInProgress;
				}
			}

			set
			{
				lock (isSynchronizationInProgressLocker)
				{
					isSynchronizationInProgress = value;
				}
			}
		}

		private bool isSynchronizationInProgress;

		private object isSynchronizationInProgressLocker = new object();

		#endregion SynchronizationInProgressLocker

        private SynchronizationManager()
            : base()
        {
			this.isSynchronizationInProgress = false;
			this.shouldCancelSynchronization = false;

			this.synchronizationInBackgroundInvocationTimer = new Timer();
			this.synchronizationInBackgroundInvocationTimer.Interval = SynchronizationInBackgroundInvocationInterval;
			this.synchronizationInBackgroundInvocationTimer.Elapsed += (object source, ElapsedEventArgs e) => 
				{
					Debug.WriteLine("INFO: Performing background sync");
					Synchronize();
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
		/// For this reason this method should be used to inform the synchronization manager that it should not save the synchronization response when synchronization is finished (usually because there are other write actions to be performed that have higher priority e.g. actions initiated by user).
		/// </summary>
		public void CancelWriteToDatabaseIfSynchronizationInProgress()
		{
			if (IsSynchronizationInProgress)
			{
				ShouldCancelSynchronization = true;
			}
		}

		/// <summary>
		/// This method downloads data from backend.
		/// @note It's caller's responsibility to check if there's a network connection available before calling this method.
		/// </summary>
		/// <returns></returns>
		public void Synchronize()
		{
			if (IsSynchronizationInProgress)
			{
				Debug.WriteLine("Info: Synchronization already in progress. Skipping synchronization.");
				return;
			}

			if (!UserSessionManager.Instance.HasStoredSession)
			{
				Debug.WriteLine("INFO: No valid session found. Skipping synchronization.");
				OnFinishedSynchronization();
				return;
			}

			if (!ReachabilityChecker.Instance.IsHostReachable(HttpConfig.Host))
			{
				Debug.WriteLine("INFO: Host " + HttpConfig.Host + " is not reachable. Skipping synchronization.");
				OnFinishedSynchronization();
				return;
			}

			try
			{
				IsSynchronizationInProgress = true;
				OnStartedSynchronization();

				FullSynchronizationTaskRun();
			}
			catch (Exception e)
			{
				OnFinishedSynchronization();
				Debug.WriteLine("Error! There was an exception during synchronization process: " + e.Message);
			}
		}

		// TODO - consider passing SynchronizationResult to OnFinishedSynchronization
		//		  (for example informing whether it was successful or not and any Error objects that occured during the process)
        private void FullSynchronizationTaskRun()
        {
            ServiceProviderSynchronization.Instance.Synchronize(
                async result => 
				{
					if (result == null)
					{
						OnFinishedSynchronization();
						return;
					}

					await Task.Run(() =>
					{
						if (ShouldCancelSynchronization)
						{
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

        private async Task InitializeDatabaseAsync()
        {
            await Task.Run(() =>
            {
                Debug.WriteLine("Copying initial databse...");
                DatabaseProvider.Setup();
                Debug.WriteLine("Finished copying initial databse.");
            });
        }

		protected virtual void OnStartedSynchronization()
		{
			if (StartedSynchronization != null)
			{
				StartedSynchronization();
			}
		}

		protected virtual void OnFinishedSynchronization()
		{
			if (IsSynchronizationInProgress)
			{
				IsSynchronizationInProgress = false;
				ShouldCancelSynchronization = false;
				if (FinishedSynchronization != null)
				{
					FinishedSynchronization();
				}
			}
		}
    }
}