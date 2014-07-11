using System;
using System.Diagnostics;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    #region Delegates

    public delegate void StartedSynchronizationEventHandler();

    public delegate void FinishedSynchronizationEventHandler();

    public delegate void UpdatedDatabaseEventHandler();

    #endregion Delegates

    public class SynchronizationManagerBase
    {
        #region Events

        public event StartedSynchronizationEventHandler StartedSynchronization;

        public event FinishedSynchronizationEventHandler FinishedSynchronization;

        public event UpdatedDatabaseEventHandler UpdatedDatabase;

        #endregion Events

        public static object locker = new Object();

        public bool IsSynchronizationInProgress
        {
            get
            {
                lock (downloadInProgressLocker)
                {
                    return isSynchronizationInProgress;
                }
            }

            set
            {
                lock (downloadInProgressLocker)
                {
                    isSynchronizationInProgress = value;
                }
            }
        }

        private bool isSynchronizationInProgress;
        public DateTime lastSuccessfulSynchronizationTimestamp = DateTime.MinValue;

        private object downloadInProgressLocker = new object();

        public SynchronizationManagerBase()
            : base()
        {
            isSynchronizationInProgress = false;
        }

        /// <summary>
        /// This method downloads Event data from backend.
        /// @note It's caller's responsibility to check if there's network connection available before calling this method.
        /// </summary>
        /// <returns></returns>
        public void Synchronize()
        {
            if (IsSynchronizationInProgress)
            {
                Debug.WriteLine("Info: Synchronization already in progress. Skipping synchronization.");
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
                throw e;
            }
        }

        /// <summary>
        /// Procedures to complete in current SynchronizationProcedure
        /// </summary>
        public virtual void FullSynchronizationTaskRun()
        {
            throw new NotImplementedException("FullSynchronization");
        }

        /// <summary>
        /// Peforms synchronization only if the elapsed time since last synchronization is greater than supplied value
        /// </summary>
        /// <param name="requiredElapsedTimeSinceLastSuccessfulSynchronization"></param>
        /// <returns></returns>
        public void SynchronizeIfElapsedTimespan(TimeSpan requiredElapsedTimeSinceLastSuccessfulSynchronization)
        {
            TimeSpan timespanSinceLastSynchronization = DateTime.Now - lastSuccessfulSynchronizationTimestamp;
            if (timespanSinceLastSynchronization > requiredElapsedTimeSinceLastSuccessfulSynchronization)
            {
                Synchronize();
            }
            else
            {
                Debug.WriteLine("INFO! Skipping sync. Actual elapsed time since last sync is " + timespanSinceLastSynchronization + " and required elapsed time is " + requiredElapsedTimeSinceLastSuccessfulSynchronization);
            }
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
                if (FinishedSynchronization != null)
                {
                    FinishedSynchronization();
                }
            }
        }

        protected virtual void OnUpdatedDatabase()
        {
            lastSuccessfulSynchronizationTimestamp = DateTime.Now;
            if (UpdatedDatabase != null)
            {
                UpdatedDatabase();
            }
        }
    }
}