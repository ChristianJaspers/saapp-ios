using MonoTouch.UIKit;

namespace BetterSalesman.iOS
{
    class CustomFlyoutNavigationController : FlyoutNavigationController
    {
        public CustomFlyoutNavigationController(UITableViewStyle navigationStyle = UITableViewStyle.Plain) : base(navigationStyle)
        {
        } 
    }
}