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
            var users = new List<User>();
            
            using (var conn = DatabaseProvider.OpenConnection())
            {
                users = conn.Table<User>().OrderBy(u => u.Experience).ToList();
            }
            
            return  users;
        }
    }
}

