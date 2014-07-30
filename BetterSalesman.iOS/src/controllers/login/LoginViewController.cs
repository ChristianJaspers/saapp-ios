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
            
            var validatorRequired = new XValidatorRequired {
                Message = I18n.ValidationRequired
            };
            
            var validator = new XForm<UITextField> {
                Inputs = new [] {
                    new XUITextField {
                        Name = I18n.FieldEmail,
                        FieldView = inputEmail,
                        Validators = new [] { validatorRequired },
                    },
                    new XUITextField {
                        Name = I18n.FieldPassword,
                        FieldView = inputPassword,
                        Validators = new [] { validatorRequired },
                    }
                }
            };
            
            loginButton.TouchUpInside += (sender, e) =>
            {
                if (validator.Validate())
                {
                    if (!IsNetworkAvailable())
                    {
                        ShowAlert(ServiceAccessError.ErrorHostUnreachable.LocalizedMessage);
                        return;
                    }
                        
                    ShowHud(I18n.AuthenticationInProgress);
                    
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
            if (!IsNetworkAvailable())
            {
                ShowAlert(ServiceAccessError.ErrorHostUnreachable.LocalizedMessage);
                return;
            }
            
            SynchronizationManagerApplication.Instance.UnsubscribeEvents();

            SynchronizationManagerApplication.Instance.StartedSynchronization += () => ShowHud(I18n.DataSynchronization);
            
            SynchronizationManagerApplication.Instance.FinishedSynchronization += () =>
            {
                HideHud();
                PerformSegue(sequeIdLogged, this);
            };

            SynchronizationManagerApplication.Instance.Synchronize();
            
        }
    }
}

