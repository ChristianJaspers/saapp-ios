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
                    inputEmail.Text, 
                    inputPsasword.Text, 
                    result =>
                    {
                        HideIndicator();
                        DismissViewController(true, null);
                        UserSessionManager.Instance.FetchUser(user=>ShowAlert("Recived token: " + user.Token));
                    },
                    errorCode =>
                    {
                        HideIndicator();
                        ShowAlert(I18n.ErrorConnectionTimeout);
                    }
                );
            };
        }
    }
}

