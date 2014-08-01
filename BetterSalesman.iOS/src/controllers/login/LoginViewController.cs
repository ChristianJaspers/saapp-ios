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
            
            inputEmail.ShouldReturn += (textField) =>
            { 
                inputEmail.ResignFirstResponder();
                inputPassword.BecomeFirstResponder();
                return true;
            };
            
            inputPassword.ShouldReturn += (textField) =>
            { 
                StartLogin();
                return true;
            };
            
            loginButton.TouchUpInside += (sender, e) => StartLogin();
            
            if (UserSessionManager.Instance.User != null)
            {
                Login();
            }
        }
        
        void StartLogin()
        {
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
            
            View.EndEditing(true);

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
                    errorMessage =>
                    {
                        HideHud();
                        ShowAlert(errorMessage);
                    }
                );
            }
            else
            {
                ShowAlert(string.Join("\n", validator.Errors));
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

