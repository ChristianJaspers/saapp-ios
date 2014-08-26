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
			if (!UserSessionManager.Instance.HasValidSession)
            {
                return null;       
            }
            
            return DatabaseHelper.Get<User>(UserSessionManager.Instance.CurrentSession.UserId);
        }
        
        public static List<User> GetUsers()
        {
			return DatabaseHelper.GetAll<User>().OrderByDescending(u => u.Experience).ToList();
        }
    }
}

