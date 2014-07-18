using System;
using MonoTouch.UIKit;
using BetterSalesman.Core.ServiceAccessLayer;
using XValidator;

namespace BetterSalesman.iOS
{
    public partial class ForgotPasswordViewController : BaseUIViewController
    {
        public ForgotPasswordViewController(IntPtr handle)
            : base(handle)
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
                    }
                }
            };
            
            skipButton.TouchUpInside += (sender, e) => DismissViewController(true, null);
            
            forgotPasswordButton.TouchUpInside += (sender, e) => {
                if ( validator.Validate() )
                {
                    ShowIndicator();
                    
                    ServiceProviderUser.Instance.ForgotPassword(
                        inputEmail.Text,
                        result => 
                        {
                            HideIndicator();
                            DismissViewController(true, null);
                            ShowAlert(I18n.SuccessMessageForgotPassword);
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

