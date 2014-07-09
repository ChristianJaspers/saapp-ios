using System;
using MonoTouch.UIKit;

namespace BetterSalesman
{
    public class BaseUIViewController : UIViewController
    {
        public BaseUIViewController(IntPtr handle) : base(handle)
        {
        }
        
        protected void PresentViewControllerWithStoryboardId(string storyboardId, bool animate = true)
        {
            UIViewController vc = (UIViewController)Storyboard.InstantiateViewController(storyboardId);

            PresentViewController(vc, animate, null);   
        }
    }
}

