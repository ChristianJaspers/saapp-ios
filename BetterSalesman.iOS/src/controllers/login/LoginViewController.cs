using System;
using MonoTouch.UIKit;

namespace BetterSalesman.iOS
{
    public partial class LoginViewController : UIViewController
    {   
        public LoginViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            loginButton.TouchUpInside += (sender, e) => DismissViewController(true, null);
        }
    }
}

