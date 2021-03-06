﻿using System;
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
            
            var validator = new XForm<UITextField> {
                Inputs = new [] {
                    new XUITextField {
                        Name = I18n.FieldEmail,
                        FieldView = inputEmail,
                        Validators = new [] {
                            new XValidatorRequired {
                                Message = I18n.ValidationRequired
                            }
                        },
                    }
                }
            };
            
            skipButton.TouchUpInside += (sender, e) => DismissViewController(true, null);
            
            forgotPasswordButton.TouchUpInside += (sender, e) => {
                if ( validator.Validate() )
                {
                    ShowHud();
                    
                    ServiceProviderUser.Instance.ForgotPassword(
                        inputEmail.Text,
                        result => 
                        {
                            HideHud();
                            DismissViewController(true, null);
                            ShowAlert(I18n.SuccessMessageForgotPassword);
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
                    ShowAlert(string.Join("\n",validator.Errors));
                }
            };
            
        }
    }
}

