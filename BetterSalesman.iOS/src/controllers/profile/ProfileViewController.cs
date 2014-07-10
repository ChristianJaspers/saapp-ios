using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;

namespace BetterSalesman.iOS
{
	partial class ProfileViewController : UIViewController
	{
		public ProfileViewController (IntPtr handle) : base (handle)
		{
		}
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            backButton.TouchUpInside += (sender, e) => DismissViewController(true, null);
        }
	}
}
