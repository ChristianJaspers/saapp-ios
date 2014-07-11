using BetterSalesman.Core.BusinessLayer;

namespace BetterSalesman.Core
{
    public class BaseSession
    {
        const string AppSessionIdentifier = "com.selleo.app";
        User user;

        static BaseSession instance;
        static object locker;
        
        public static BaseSession Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new BaseSession();
                        }
                    }
                }

                return instance;
            }
        }
        
        void Save()
        {
            // save user with error handling?
        }
        
        void Discard()
        {
            // clean user
            // remove from session data
        }
        
        User User
        {
            get {
                // TODO fetching
                user = new User();
                
                return user;
            }
            set {
                user = value;   
            }
        }
    }
}