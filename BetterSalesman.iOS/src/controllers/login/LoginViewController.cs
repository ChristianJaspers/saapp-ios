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
                ShowIndicator();
                
                ServiceProviderUser.Instance.Authentication(
                    "john", 
                    "doe", 
                    result =>
                    {
                        HideIndicator();
                        DismissViewController(true, null);
                    },
                    errorCode =>
                    {
                        HideIndicator();
                        ShowAlert("Kod błędu: " + errorCode,"Wystąpił problem");
                        DismissViewController(true, null);
                    }
                    // TODO we leave it for testing purpose since backend is not ready
                );
            };
        }
    }
}

