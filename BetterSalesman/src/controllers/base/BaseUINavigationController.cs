using MonoTouch.UIKit;
using System;
using MonoTouch.Foundation;

namespace BetterSalesman
{
    [Register("BaseUINavigationController")]
    public class BaseUINavigationController : UINavigationController
    {
        public string InitialControllerType;
        
        public BaseUINavigationController(IntPtr handle) : base(handle)
        {
        }
    }
}

