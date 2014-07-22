using BetterSalesman.Core.DataLayer;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.Core.BusinessLayer.Managers
{
    public static class UserManager
    {        
        public static User LoggedInUser()
        {
            return DatabaseHelper.Get<User>(UserSessionManager.Instance.User.UserId);
        }
    }
}

