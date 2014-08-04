﻿using BetterSalesman.Core.DataLayer;
using BetterSalesman.Core.ServiceAccessLayer;
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
        
        public static List<User> Users()
        {
            return DatabaseHelper.GetAll<User>();
        }
    }
}

