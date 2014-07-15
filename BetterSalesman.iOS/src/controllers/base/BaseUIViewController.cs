using System;
using MonoTouch.UIKit;
using MBProgressHUD;

namespace BetterSalesman.iOS
{
    public class BaseUIViewController : UIViewController
    {
        static MTMBProgressHUD hud;
        public BaseUIViewController(IntPtr handle) : base(handle)
        {
        }
        
        protected void PresentViewControllerWithStoryboardId(string storyboardId, bool animate = true)
        {
            UIViewController vc = (UIViewController)Storyboard.InstantiateViewController(storyboardId);

            PresentViewController(vc, animate, null);   
        }
        
        protected void ShowProgressIndicator()
        {
            hud = new MTMBProgressHUD (View) {
                RemoveFromSuperViewOnHide = true
            };

            View.Superview.AddSubview (hud);

            hud.Show (true);
        }
        
        protected void HideProgressIndicator()
        {
            if (hud != null && !hud.Hidden)
            {
                hud.Hide(true,1);
            }
        }
    }
}

