using System;
using MonoTouch.UIKit;
using FlyoutNavigation;
using System.Collections.Generic;
using MonoTouch.Dialog;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.iOS
{
    public partial class RootViewController : BaseUIViewController
    {
        public static FlyoutNavigationController Navigation;
        
        const string navigationBase = "NavigationBase";
        const string navigationMyTeam = "NavigationMyTeam";

        public RootViewController(IntPtr handle) : base(handle)
        {
        }

        #region View lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            NavigationLayoutInit();
        }

        #endregion
        
        #region Flayout Navigation
        
        void NavigationLayoutInit()
        {
            Navigation = new FlyoutNavigationController();
            Navigation.View.Frame = UIScreen.MainScreen.Bounds;
            Navigation.AlwaysShowLandscapeMenu = false;
            Navigation.ShouldReceiveTouch += (recognizer, touch) => false;
            View.AddSubview(Navigation.View);

            PopulateNavigationItems();
        }

        void PopulateNavigationItems()
        {
            var elements = new List<FlayoutNavigationItem>();

            elements.Add(
                new FlayoutNavigationItem(
                    I18n.Profile, 
                    Profile,
                    UIImage.FromBundle(""), // TODO icons here
                    navigationBase
                )
            );
            
            elements.Add(
                new FlayoutNavigationItem(
                    I18n.MyTeam, 
                    MyTeam,
                    UIImage.FromBundle(""), // TODO icons here
                    navigationMyTeam
                )
            );
            
            elements.Add(
                new FlayoutNavigationItem(
                    I18n.Logout, 
                    Logout,
                    UIImage.FromBundle(""), // TODO icons here
                    navigationBase
                )
            );

            string[] controllers = new string[elements.Count];

            int a = 0;
            foreach (var element in elements)
            {
                controllers[a] = element.Controller;
                a++;
            }

            Navigation.NavigationRoot = new RootElement("") { new Section { elements } };

            Navigation.ViewControllers = Array.ConvertAll(controllers, title => controllerForSection(title));
        }
        
        UIViewController controllerForSection(string title = navigationBase)
        {
            BaseUINavigationController vc = (BaseUINavigationController)Storyboard.InstantiateViewController(title);
            vc.InitialControllerType = title;
            return vc;
        }

        #endregion
        
        void Profile()
        {
            PresentViewControllerWithStoryboardId("Profile");
        }
        
        void MyTeam()
        {
            // load it here
        }
        
        void Logout()
        {
            UserSessionManager.Instance.Discard();

            UIViewController vc = (UIViewController)Storyboard.InstantiateViewController("Login");

            PresentViewController(vc, false, null);
        }
        
    }
}

