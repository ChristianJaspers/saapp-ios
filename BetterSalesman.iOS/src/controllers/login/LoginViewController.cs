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
                if ( Validate() )
                {
                    ShowIndicator();
                    
                    ServiceProviderUser.Instance.Authentication(
                        inputEmail.Text, 
                        inputPassword.Text, 
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
                }
                else
                {
                    ShowAlert(I18n.ErrorFillFields);
                }
            };
        }

        bool Validate()
        {   
            // TODO we need build some serious validator
            InvokeOnMainThread(() =>
            {
                inputEmail.Layer.BorderWidth = 1;
                inputPassword.Layer.BorderWidth = 1;
                inputEmail.Layer.BorderColor = inputEmail.IsNotEmpty() && inputEmail.IsValidEmail() ? UIColor.LightGray.CGColor : UIColor.Red.CGColor;
                inputPassword.Layer.BorderColor = inputPassword.IsNotEmpty() ? UIColor.LightGray.CGColor : UIColor.Red.CGColor;
            });
            
            return inputEmail.IsNotEmpty() && inputEmail.IsValidEmail() && inputPassword.IsNotEmpty();
        }
    }
}

