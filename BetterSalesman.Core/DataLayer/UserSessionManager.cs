using BetterSalesman.Core.BusinessLayer;
using System;
using Akavache;
using ReactiveUI;
using System.Threading.Tasks;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public class UserSessionManager
    {
        const string CacheUserKey = "user";
        UserSession user;

        static UserSessionManager instance;
        static object locker = new Object();
        
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
        
        public void Save()
        {
            BlobCache.UserAccount.InsertObject(CacheUserKey, user, DateTimeOffset.MaxValue);
        }
        
        public void Discard()
        {
            user = null;
            
            BlobCache.UserAccount.InvalidateObject<UserSession>(CacheUserKey);
        }
        
        public async Task FetchUser(Action<UserSession> finished = null)
        {
            user = await BlobCache.UserAccount.GetObjectAsync<UserSession>(CacheUserKey);
            
            if (finished != null)
            {
                finished(user);
            }
        }
        
        public UserSession User
        {
            get {
                return user;
            }
            set {
                user = value;   
            }
        }
    }
}