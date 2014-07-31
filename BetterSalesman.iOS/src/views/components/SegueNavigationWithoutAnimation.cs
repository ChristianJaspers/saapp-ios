using System;

using MonoTouch.UIKit;

namespace BetterSalesman.iOS
{
	public partial class SegueNavigationWithoutAnimation : UIStoryboardSegue
	{
		public SegueNavigationWithoutAnimation (IntPtr handle) : base (handle)
		{
		}
        
        public override void Perform()
        {
            SourceViewController.NavigationController.PushViewController(DestinationViewController, false);
        }
	}
}
