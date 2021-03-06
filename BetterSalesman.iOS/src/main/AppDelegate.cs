﻿
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        public static UIColor TintColor = UIColor.FromRGB(116,123,129);
        
        public static UIColor ColorTextGreen = UIColor.Clear.FromHex("#50AA1D");
        public static UIColor ColorTextGray = UIColor.Clear.FromHex("#3D3D3D");
        public static UIColor ColorTextProductGroupPicker = UIColor.White;
        public static UIColor ColorBackgroundGreen = UIColor.Clear.FromHex("#50AA1D");
        public static UIColor ColorBackgroundGray = UIColor.Clear.FromHex("#e6e6e6");
        public static UIColor ColorTextDarkGray = UIColor.Clear.FromHex("#2b2b2b");
        public static UIColor ColorTextLightGray = UIColor.Clear.FromHex("#878e94");

        public static UIColor ColorOrange = UIColor.Clear.FromHex("#F3810B");
        
        public static UIColor ColorBackgroundZebraOdd = UIColor.White;
        public static UIColor ColorBackgroundZebraEven = UIColor.White;
        
        public static UIColor RatingLowColor = UIColor.Clear.FromHex("#f74343");
        public static UIColor RatingMediumColor = UIColor.Clear.FromHex("#f89406");
        public static UIColor RatingHighColor = UIColor.Clear.FromHex("#60b528");
        
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
            
            CustomStyles.Init();

			return true;
		}

        // This method is invoked when the application is about to move from active to inactive state.
        // OpenGL applications should use this method to pause.
        public override void OnResignActivation(UIApplication application)
        {
        }

		public override void OnActivated(UIApplication application)
		{
			SynchronizationManager.Instance.StartSynchronizationInBackgroundTimer();
			SynchronizationManager.Instance.Synchronize(isBackgroundSynchronization: true);
		}

        // This method should be used to release shared resources and it should store the application state.
        // If your application supports background exection this method is called instead of WillTerminate
        // when the user quits.
        public override void DidEnterBackground(UIApplication application)
        {
			SynchronizationManager.Instance.StopSynchronizationInBackgroundTimer();
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

