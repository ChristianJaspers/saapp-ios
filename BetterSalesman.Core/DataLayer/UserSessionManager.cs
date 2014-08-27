using BetterSalesman.Core.BusinessLayer;
using System;
using Akavache;
using ReactiveUI;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public class UserSessionManager
    {
        private const string CacheKeySession = "session";

        private static UserSessionManager instance;
        private static object locker = new Object();
        
        public static UserSessionManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new UserSessionManager();
                        }
                    }
                }

                return instance;
            }
        }
        
		/// <summary>
		/// Save CurrentSession to database.
		/// </summary>
        private void Save()
        {
            BlobCache.UserAccount.InsertObject(CacheKeySession, CurrentSession, DateTimeOffset.MaxValue);
        }
        
        public void Discard()
        {
            CurrentSession = null;
            
            BlobCache.UserAccount.InvalidateObject<UserSession>(CacheKeySession);
        }

		/// <summary>
		/// Saves session created from userId and accessToken to database.
		/// </summary>
		/// <param name="userId">BetterSalesman user identifier in BetterSalesman database (session and business data are stored in separate databases).</param>
		/// <param name="accessToken">Access token for BetterSalesman service.</param>
		public void SaveSession(int userId, string accessToken)
		{
			if (string.IsNullOrEmpty(accessToken))
			{
				Debug.WriteLine("ERROR! Save session requires accessToken to be non-null. Cancelling saving session.");
				return;
			}

			CurrentSession = new UserSession
			{
				UserId = userId,
				Token = accessToken
			};

			Save();
		}

        public async Task<UserSession> LoadSessionAsync()
        {
            try
            {
				CurrentSession = await BlobCache.UserAccount.GetObjectAsync<UserSession>(CacheKeySession);

				return CurrentSession;
            } 
            catch (Exception ex)
            {
				Debug.WriteLine("Failed to fetch user from database: " + ex.Message + "\n" + ex.StackTrace);
                return null;
            }
        }

		public bool HasStoredSession
		{
			get
			{
				return CurrentSession != null;
			}
		}
        
        public UserSession CurrentSession
        {
			get;
			set;
        }
        
        public string AccessToken
        {
            get 
            {
                if (CurrentSession != null)
                {
                    return CurrentSession.Token;
                }
                
                return string.Empty;
            }
        }
    }
}