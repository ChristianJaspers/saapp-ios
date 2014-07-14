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
                ServiceProviderUser.Instance.Authentication(
                    "john",
                    "doe",
                    result => {
                        DismissViewController(true, null);
//                    
                        UserSessionManager.Instance.User = new UserSession {
                            Token = "123",
                        };
                        // we can do some serialization here and save user?
                    }
                );
                
                UserSessionManager.Instance.Save();
            };
        }
    }
}

