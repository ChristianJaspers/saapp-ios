using System;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BetterSalesman.Core.DataLayer;
using BetterSalesman.Core.ServiceAccessLayer;
using System.Diagnostics;

namespace BetterSalesman.iOS
{
	partial class MainViewController : UINavigationController
	{
		public MainViewController (IntPtr handle) : base (handle)
		{
		}
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            DatabaseProvider.Setup();
        }
	}
}
