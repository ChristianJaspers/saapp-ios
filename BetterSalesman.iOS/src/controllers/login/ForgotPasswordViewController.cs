using System;
using MonoTouch.UIKit;

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
        }
    }
}

