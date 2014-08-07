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
        
        const string VEmpty = "Empty";
        const string VProfile = "Profile";
        const string VListArguments = "ListArguments";
        const string VListMyTeam = "ListMyTeam";
        
        const string storyboardIdLogin = "Login";
        
        const string segueIDLogout = "LogoutSegue";
        
        // TODO update icons
        const string IcProfile = "";
        const string IcArguments = "";
        const string IcMyTeam = "";
        const string IcLogout = "";
        const string IcSynchronization = "";

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
        
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            Navigation.SelectedIndex = 1;
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
                    null,
                    UIImage.FromBundle(IcProfile),
                    VProfile
                ),
                new FlayoutNavigationItem(
                    I18n.Arguments, 
                    null,
                    UIImage.FromBundle(IcArguments),
                    VListArguments
                ),
                new FlayoutNavigationItem(
                    I18n.MyTeam, 
                    null,
                    UIImage.FromBundle(IcMyTeam),
                    VListMyTeam
                ),
                new FlayoutNavigationItem(
                    I18n.Synchronization, 
                    Synchronization,
                    UIImage.FromBundle(IcSynchronization),
                    VEmpty
                ),
                new FlayoutNavigationItem(
                    I18n.Logout, 
                    Logout,
                    UIImage.FromBundle(IcLogout),
                    VEmpty
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
            if (title.Equals(VEmpty))
            {
                return null;
            }
            
            BaseUINavigationController vc = (BaseUINavigationController)Storyboard.InstantiateViewController(storyboardIdNavigationBase);
            
            vc.InitialControllerType = title;
            
            return vc;
        }

        #endregion
        
        #region Tab actions

        void Logout()
        {
            UserSessionManager.Instance.Discard();
            
            PerformSegue(segueIDLogout, this);
            
        }

        void Synchronization()
        {
            if (!IsNetworkAvailable())
            {
                ShowAlert(ServiceAccessError.ErrorHostUnreachable.LocalizedMessage);
                return;
            }
            
            SynchronizationManagerApplication.Instance.Synchronize();
        }
        
        protected override void OnSynchronizationStart()
        {
            base.OnSynchronizationStart();

            ShowHud(I18n.SynchronizationInProgress);
        }

        protected override void OnSynchronizationFinished()
        {
            base.OnSynchronizationFinished();

            HideHud();
            
            Navigation.HideMenu();
        }
        
        #endregion
    }
}

