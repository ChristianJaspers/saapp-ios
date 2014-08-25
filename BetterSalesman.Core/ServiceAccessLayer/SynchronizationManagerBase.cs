using System;
using System.Diagnostics;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    #region Delegates

    public delegate void StartedSynchronizationEventHandler();

    public delegate void FinishedSynchronizationEventHandler();

    #endregion Delegates

    public class SynchronizationManagerBase
    {
        #region Events

        public event StartedSynchronizationEventHandler StartedSynchronization;

        public event FinishedSynchronizationEventHandler FinishedSynchronization;

        #endregion Events

        public static object locker = new Object();

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

        public SynchronizationManagerBase()
            : base()
        {
            isSynchronizationInProgress = false;
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
    }
}