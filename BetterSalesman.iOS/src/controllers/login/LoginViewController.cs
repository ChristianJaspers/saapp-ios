using System;
using MonoTouch.UIKit;
using BetterSalesman.Core.ServiceAccessLayer;
using XValidator;

namespace BetterSalesman.iOS
{
    public partial class LoginViewController : BaseUIViewController
    {   
        const string sequeIdLogged = "LoggedSeque";
        public LoginViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            var validator = new XFormValidator<UITextField> {
                Inputs = new [] {
                    new XUITextFieldValidate {
                        Name = I18n.FieldEmail,
                        FieldView = inputEmail,
                        Validators = new [] {
                            new XValidatorRequired()
                        },
                    },
                    new XUITextFieldValidate {
                        Name = I18n.FieldPassword,
                        FieldView = inputPassword,
                        Validators = new [] {
                            new XValidatorRequired()
                        },
                    }
                }
            };
            
            loginButton.TouchUpInside += (sender, e) =>
            {
                if ( validator.Validate() )
                {
                    ShowHud();
                    
                    ServiceProviderUser.Instance.Authentication(
                        inputEmail.Text, 
                        inputPassword.Text, 
                        result =>
                        {
                            HideHud();
                            Login();
//                            UserSessionManager.Instance.FetchUser(user=>ShowAlert("Recived token: " + user.Token));
                        },
                        errorCode =>
                        {
                            HideHud();
                            ShowAlert(I18n.ErrorConnectionTimeout);
                        }
                    );
                }
                else
                {
                    ShowAlert(string.Join("\n",validator.Errors));
                }
            };
            
            UserSessionManager.Instance.FetchUser(user => 
            {
                if ( user != null )
                {
                    Login();
                }  
            });
        }
        
        void Login()
        {
            PerformSegue(sequeIdLogged, this);
        }
    }
}

