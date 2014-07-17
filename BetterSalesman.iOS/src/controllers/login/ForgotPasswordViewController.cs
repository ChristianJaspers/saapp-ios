using System;
using MonoTouch.UIKit;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.iOS
{
    public partial class ForgotPasswordViewController : UIViewController
    {   
        public ForgotPasswordViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            skipButton.TouchUpInside += (sender, e) => DismissViewController(true, null);
            
            // TODO request here
            ServiceProviderUser.Instance.ForgotPassword(
                "email@here.pl",
                result =>
                {
                    
                },
                errorCode =>
                {
                
                }
            );
        }
    }
}

