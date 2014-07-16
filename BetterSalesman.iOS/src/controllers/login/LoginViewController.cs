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
                        UserSessionManager.Instance.FetchUser(user=>ShowAlert("Recived token: " + user.Token));
                    },
                    errorCode =>
                    {
                        HideIndicator();
                        ShowAlert("Error code: " + errorCode,"Problem");
                        DismissViewController(true, null);
                    }
                    // TODO we leave it for testing purpose since backend is not ready
                );
            };
        }
    }
}

