using BetterSalesman.Core.DataLayer;
using BetterSalesman.Core.ServiceAccessLayer;
using System.Linq;
using System.Collections.Generic;

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
        
        public static List<User> GetUsers()
        {
            return DatabaseHelper.GetAll<User>().OrderBy(u=>u.Experience).ToList();
        }
    }
}

