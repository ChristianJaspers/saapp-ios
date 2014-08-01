// This file has been autogenerated from a class added in the UI designer.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.iOS
{
    public partial class AppEntryViewController : UIViewController
    {
        const string segueIDLoggedIn = "LoggedInSegue";
        const string segueIDNotLoggedIn = "NotLoggedInSegue";

        public AppEntryViewController(IntPtr handle)
            : base(handle)
        {
        }
        
        public async override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            
            var user = await UserSessionManager.Instance.UserAsync();
            
            UserSessionManager.Instance.User = user;

            string segueToPerform = user != null ? segueIDLoggedIn : segueIDNotLoggedIn;

            System.Diagnostics.Debug.WriteLine("Segue to perform: " + segueToPerform);

            PerformSegue(segueToPerform, this);
        }
    }
}
