using BetterSalesman.Core.DataLayer;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.Core.BusinessLayer.Managers
{
    public static class UserManager
    {        
        public static User LoggedInUser()
        {
            if (UserSessionManager.Instance.User == null)
            {
                return null;       
            }
            
            return DatabaseHelper.Get<User>(UserSessionManager.Instance.User.UserId);
        }
    }
}

