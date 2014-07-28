using System;
using MonoTouch.UIKit;
using BetterSalesman.Core.ServiceAccessLayer;
using XValidator;

namespace BetterSalesman.iOS
{
    public partial class LoginViewController : BaseUIViewController
    {
        const string sequeIdLogged = "LoggedSeque";

        public LoginViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            var validator = new XForm<UITextField> {
                Inputs = new [] {
                    new XUITextField {
                        Name = I18n.FieldEmail,
                        FieldView = inputEmail,
                        Validators = new [] {
                            new XValidatorRequired()
                        },
                    },
                    new XUITextField {
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
                if (validator.Validate())
                {
                    ShowHud();
                    
                    ServiceProviderUser.Instance.Authentication(
                        inputEmail.Text, 
                        inputPassword.Text, 
                        result =>
                        {
                            HideHud();
                            Login();
                        },
                        errorCode =>
                        {
                            HideHud();
                            ShowAlert(I18n.ErrorConnectionTimeout);
                        }
                    );
                } else
                {
                    ShowAlert(string.Join("\n", validator.Errors));
                }
            };
            
            if (UserSessionManager.Instance.User != null)
            {
                Login();
            }
        }

        void Login()
        {
            SynchronizationManagerApplication.Instance.UnsubscribeEvents();

            SynchronizationManagerApplication.Instance.StartedSynchronization += () => ShowHud();
            
            SynchronizationManagerApplication.Instance.FinishedSynchronization += () =>
            {
                HideHud();
                PerformSegue(sequeIdLogged, this);
            };

            SynchronizationManagerApplication.Instance.Synchronize();
            
        }
    }
}

