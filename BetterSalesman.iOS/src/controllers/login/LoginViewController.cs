using System;
using MonoTouch.UIKit;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.iOS
{
    public partial class LoginViewController : BaseUIViewController
    {   
        public LoginViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            loginButton.TouchUpInside += (sender, e) =>
            {
                ShowProgressIndicator();
                
                ServiceProviderUser.Instance.Authentication(
                    "john", 
                    "doe", 
                    result =>
                    {
                        HideProgressIndicator();
                        DismissViewController(true, null);
                    },
                    errorCode =>
                    {
                        HideProgressIndicator();
                        DismissViewController(true, null);
                    }
                    // TODO we leave it for testing purpose since backend is not ready
                );
            };
        }
    }
}

