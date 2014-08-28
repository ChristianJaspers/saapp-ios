
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BetterSalesman.Core.ServiceAccessLayer;
using System.Diagnostics;

namespace BetterSalesman.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        public static UIColor TintColor = UIColor.FromRGB(116,123,129);
        
        public static UIColor ColorTextOrange = UIColor.Clear.FromHex("#f76500");
        public static UIColor ColorTextGray = UIColor.Clear.FromHex("#3D3D3D");
        public static UIColor ColorBackgroundOrange = UIColor.Clear.FromHex("#f76500");
        public static UIColor ColorBackgroundGray = UIColor.Clear.FromHex("#e6e6e6");
        
        public static UIColor ColorBackgroundZebraOdd = UIColor.Clear.FromHex("#f8f8f8");
        public static UIColor ColorBackgroundZebraEven = UIColor.Clear.FromHex("#FFFFFF");
        
        public override UIWindow Window
        {
            get;
            set;
        }
        
		public override bool WillFinishLaunching(UIApplication application, NSDictionary launchOptions)
		{
			Localization.Instance.Initialize(new IosLocalizationProvider());
			ReachabilityChecker.Instance.Initialize(new IosReachabilityProvider());
            
            Window.TintColor = TintColor;
            
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes() {
                TextColor = ColorTextOrange,
                TextShadowOffset = new UIOffset(0,0),
                Font = UIFont.FromName("HelveticaNeue-Light",20)
            });

			return true;
		}

        // This method is invoked when the application is about to move from active to inactive state.
        // OpenGL applications should use this method to pause.
        public override void OnResignActivation(UIApplication application)
        {
        }

		public override void OnActivated(UIApplication application)
		{
			SynchronizationManagerApplication.Instance.StartSynchronizationInBackgroundTimer();
		}

        // This method should be used to release shared resources and it should store the application state.
        // If your application supports background exection this method is called instead of WillTerminate
        // when the user quits.
        public override void DidEnterBackground(UIApplication application)
        {
			SynchronizationManagerApplication.Instance.StopSynchronizationInBackgroundTimer();
        }
        
        // This method is called as part of the transiton from background to active state.
        public override void WillEnterForeground(UIApplication application)
        {
        }
        
        // This method is called when the application is about to terminate. Save data, if needed.
        public override void WillTerminate(UIApplication application)
        {
        }
    }
}

