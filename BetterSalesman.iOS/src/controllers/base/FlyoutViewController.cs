using System;
using MonoTouch.UIKit;
using FlyoutNavigation;
using System.Collections.Generic;
using MonoTouch.Dialog;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.iOS
{
    public partial class FlyoutViewController : BaseUIViewController
    {
        public static FlyoutNavigationController Navigation;
        
        const string storyboardIdNavigationBase = "NavigationBase";
        
        const string VListArguments = "ListArguments";
        const string VListMyTeam = "ListMyTeam";
        
        const string storyboardIdProfile = "Profile";
        const string storyboardIdLogin = "Login";
        
        const string segueIDLogout = "LogoutSegue";

        public FlyoutViewController(IntPtr handle)
            : base(handle)
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
            var elements = new List<FlayoutNavigationItem>() {
                new FlayoutNavigationItem(
                    I18n.Profile, 
                    Profile,
                    UIImage.FromBundle(""), // TODO icons here
                    VListArguments
                ),
                new FlayoutNavigationItem(
                    I18n.Arguments, 
                    null,
                    UIImage.FromBundle(""), // TODO icons here
                    VListArguments
                ),
                new FlayoutNavigationItem(
                    I18n.MyTeam, 
                    null,
                    UIImage.FromBundle(""), // TODO icons here
                    VListMyTeam
                ),
                new FlayoutNavigationItem(
                    I18n.Logout, 
                    Logout,
                    UIImage.FromBundle(""), // TODO icons here
                    VListArguments
                )
            };

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

        UIViewController controllerForSection(string title = storyboardIdNavigationBase)
        {
            BaseUINavigationController vc = (BaseUINavigationController)Storyboard.InstantiateViewController(storyboardIdNavigationBase);
            vc.InitialControllerType = title;
            return vc;
        }
        
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            
            Navigation.SelectedIndex = 1;
        }

        #endregion

        void Profile()
        {
            PresentViewControllerWithStoryboardId(storyboardIdProfile);
        }

        void Logout()
        {
            UserSessionManager.Instance.Discard();
            
            PerformSegue(segueIDLogout, this);
            
        }
    }
}

