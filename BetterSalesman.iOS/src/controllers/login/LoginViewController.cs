using System;
using MonoTouch.UIKit;
using BetterSalesman.Core.ServiceAccessLayer;
using BetterSalesman.Core.BusinessLayer;

namespace BetterSalesman.iOS
{
    public partial class LoginViewController : UIViewController
    {   
        public LoginViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            loginButton.TouchUpInside += (sender, e) =>
            {
                DismissViewController(true, null);                
                
                UserSessionManager.Instance.User = new User {
                    Id = 1,
                    Username = "John",
                    Email = "Test user",
                    Token = "abcdxyz"
                };
                
                UserSessionManager.Instance.Save();
            };
        }
    }
}

