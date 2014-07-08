using System;
using MonoTouch.UIKit;

namespace BetterSalesman
{
    public partial class ForgotPasswordViewController : UIViewController
    {   
        public ForgotPasswordViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
        
        partial void Skip (UIButton sender)
        {
            DismissViewController(true, null);
        }
    }
}

