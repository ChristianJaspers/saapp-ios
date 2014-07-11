using BetterSalesman.Core.BusinessLayer;
using System;
using ReactiveUI;
using Akavache;
using System.Threading.Tasks;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public class UserSessionManager
    {
        const string CacheUserKey = "user";
        User user;

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
            
            BlobCache.UserAccount.InvalidateObject<User>(CacheUserKey);
        }
        
        public async Task FetchUser(Action finished = null)
        {
            user = await BlobCache.UserAccount.GetObjectAsync<User>(CacheUserKey);
            
            if (finished != null)
            {
                finished();
            }
        }
        
        public User User
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