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
            
            buttonForgotPassword.TouchUpInside += (s, e) => DisplayPasswordChange(string.Empty, true);
            buttonForgotPassword.SetTitle(I18n.ForgotPassword, UIControlState.Normal);
            
            inputEmail.Placeholder = I18n.FieldEmail;
            inputPassword.Placeholder = I18n.FieldPassword;
            
            inputEmail.ShouldReturn += textField =>
            { 
                inputEmail.ResignFirstResponder();
                inputPassword.BecomeFirstResponder();
                return true;
            };
            
            inputPassword.ShouldReturn += textField =>
            { 
                StartLogin();
                return true;
            };
            
			loginButton.SetTitle(I18n.Login, UIControlState.Normal);
            loginButton.TouchUpInside += (sender, e) => StartLogin();
            
			if (UserSessionManager.Instance.HasValidSession)
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

            SynchronizationManagerApplication.Instance.Synchronize();
            
        }
        
        protected override void OnSynchronizationStart()
        {
            base.OnSynchronizationStart();
            
            ShowHud(I18n.SynchronizationInProgress);
        }
        
        protected override void OnSynchronizationFinished()
        {
            base.OnSynchronizationFinished();
            
            HideHud();
            
            PerformSegue(sequeIdLogged, this);
        }
        
        #region Forgot password

        XForm<UITextField> validator;

        void DisplayPasswordChange(string defaultEmail = "", bool firstTime = false)
        {   
            UIAlertView alert = new UIAlertView(I18n.ProvideYourEmail, string.Empty, null, I18n.OK, I18n.Cancel);

            alert.AlertViewStyle = UIAlertViewStyle.PlainTextInput;

            var emailField = alert.GetTextField(0);

            if ( !firstTime )
            {
                emailField.Text = defaultEmail;
                ValidateField(emailField);
            }

            alert.Clicked += (sender, e) =>
                {
                    if ( e.ButtonIndex == 0 )
                    {       
                        if ( ValidateField(emailField) )
                        {
                            ResetMyPassword(emailField.Text);
                        }
                        else
                        {
                            ShowAlert( string.Join("\n", validator.Errors),() => DisplayPasswordChange(emailField.Text) );
                        }
                    }
                };

            alert.Show();
        }

        bool ValidateField(UITextField field)
        {
            validator = new XForm<UITextField> {
                Inputs = new [] {
                    new XUITextField {
                        Name = I18n.FieldEmail,
                        FieldView = field,
                        Validators = new [] {
                            new XValidatorRequired { Message = I18n.ValidationRequired }
                        },
                    }
                }
            };

            return validator.Validate();
        }

        void ResetMyPassword(string email)
        {
            if (!IsNetworkAvailable())
            {
                ShowAlert(ServiceAccessError.ErrorHostUnreachable.LocalizedMessage);
                return;
            }

            ShowHud();

            ServiceProviderUser.Instance.ForgotPassword(
                email,
                result => 
                {
                    HideHud();
                    ShowAlert(I18n.SuccessMessageForgotPassword);
                },
                errorMessage =>
                {
                    HideHud();
                    ShowAlert(errorMessage);
                }
            );
        }

        #endregion
    }
}

