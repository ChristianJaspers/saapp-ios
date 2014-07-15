using System;
using MonoTouch.UIKit;
using BetterSalesman.Core.ServiceAccessLayer;

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
                ServiceProviderUser.Instance.Authentication(
                    "john", 
                    "doe", 
                    result => DismissViewController(true, null),
                    errorCode => DismissViewController(true, null) 
                    // TODO we leave it for testing purpose since backend is not ready
                );
        }
    }
}

