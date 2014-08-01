using MonoTouch.UIKit;
using System;
using System.Linq;
using MonoTouch.Foundation;
using System.Collections.Generic;

namespace BetterSalesman.iOS
{
    [Register("BaseUINavigationController")]
    public class BaseUINavigationController : UINavigationController
    {
        public string InitialControllerType;
        
        public BaseUINavigationController(IntPtr handle) : base(handle)
        {
        }
        
        public UIViewController vc;
        
        public override void ViewDidLoad()
        {
            vc = (UIViewController)Storyboard.InstantiateViewController(InitialControllerType);

            PushViewController(vc, false);
        }
    }
}

