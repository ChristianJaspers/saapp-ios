using System;
using System.Diagnostics;
using MonoTouch.UIKit;
using BetterSalesman.Core.ServiceAccessLayer;
using XValidator;

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
            
            var validator = new XFormValidator<UITextField> {
                Inputs = new XFieldValidate<UITextField>[] {
                    new XUITextFieldValidate {
                        Name = "Email".t(),
                        FieldView = inputEmail,
                        Validators = new XValidatorBase[] {
                            new XValidatorRequired()
                        },
                    },
                    new XUITextFieldValidate {
                        Name = "Password".t(),
                        FieldView = inputPassword,
                        Validators = new XValidatorBase[] {
                            new XValidatorRequired()
                        },
                    }
                }
            };
            
            loginButton.TouchUpInside += (sender, e) =>
            {
                if ( validator.Validate() )
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
                    ShowAlert(string.Join("\n",validator.Errors));
                }
            };
        }
    }
}

