using System;
using MonoTouch.UIKit;
using MBProgressHUD;

namespace BetterSalesman.iOS
{
    public class BaseUIViewController : UIViewController
    {
        MTMBProgressHUD hud;
        public BaseUIViewController(IntPtr handle) : base(handle)
        {
        }
        
        protected void PresentViewControllerWithStoryboardId(string storyboardId, bool animate = true)
        {
            UIViewController vc = (UIViewController)Storyboard.InstantiateViewController(storyboardId);

            PresentViewController(vc, animate, null);   
        }
        
        protected void ShowIndicator()
        {
            hud = new MTMBProgressHUD (View) {
                RemoveFromSuperViewOnHide = true
            };

            View.Superview.AddSubview (hud);

            hud.Show (true);
        }
        
        protected void HideIndicator()
        {
            if (hud != null && !hud.Hidden)
            {
                hud.Hide(true,0.5);
            }
        }
        
        protected void ShowAlert(string msg)
        {
            ShowAlert(msg, string.Empty);
        }
        
        protected void ShowAlert(string msg, string title)
        {
            new UIAlertView (title, msg, null, I18n.OK, null).Show ();
        }
    }
}

