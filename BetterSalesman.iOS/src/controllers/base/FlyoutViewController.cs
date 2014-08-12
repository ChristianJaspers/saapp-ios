using System;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.Dialog;
using MonoTouch.Foundation;
using BetterSalesman.Core.ServiceAccessLayer;
using System.Threading;

namespace BetterSalesman.iOS
{
    public partial class FlyoutViewController : BaseUIViewController
    {
		public static CustomFlyoutNavigationController Navigation;
        
        FlayoutNavigationItemProfile profileElement;
        
        const string storyboardIdNavigationBase = "NavigationBase";
        
        const string VEmpty = "Empty";
        const string VProfile = "Profile";
        const string VListArguments = "ListArguments";
        const string VListMyTeam = "ListMyTeam";
        
        const string storyboardIdLogin = "Login";
        
        const string segueIDLogout = "LogoutSegue";
        
        const string IcArguments = "ic_arguments";
        const string IcMyTeam = "ic_team";
        const string IcLogout = "ic_logout";
        
        // TODO remove synchronization button
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
            
            SelectTab(0);
        }

        #endregion
        
        #region Flayout Navigation
        
        void NavigationLayoutInit()
        {
            Navigation = new CustomFlyoutNavigationController();
            Navigation.View.Frame = UIScreen.MainScreen.Bounds;
            Navigation.AlwaysShowLandscapeMenu = false;
            Navigation.ShadowViewColor = UIColor.FromRGBA(0,0,0,48);
            Navigation.ShouldReceiveTouch += (r, t) => false;
            View.AddSubview(Navigation.View);

			Navigation.MenuWasShown += () =>
			{
				profileElement.RefreshUserData();
			};

            PopulateNavigationItems();
        }

        void PopulateNavigationItems()
        {
            profileElement = new FlayoutNavigationItemProfile(string.Empty,VProfile);
            
            var elements = new List<Element>() {
                profileElement,
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
                    I18n.Logout, 
                    Logout,
                    UIImage.FromBundle(IcLogout),
                    VEmpty
                ),
                new FlayoutNavigationItem(
                    I18n.Synchronization, 
                    Synchronization,
                    UIImage.FromBundle(IcSynchronization),
                    VEmpty
                )
            };

            string[] controllers = new string[elements.Count];

            int a = 0;
            foreach (var element in elements)
            {
                controllers[a] = (element as IFlyoutNavigationItem).Controller;
                a++;
            }

            Navigation.NavigationRoot = new RootElement("") { new Section { elements } };

            Navigation.NavigationRoot.TableView.BackgroundColor = AppDelegate.ColorBackgroundGray;
            
            Navigation.NavigationRoot.TableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            
            Navigation.NavigationRoot.TableView.Source = new UIFLyoutNavigationSource(elements);
            
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

        int lastSelectedIndex;
        
        protected override void OnSynchronizationStart()
        {
            base.OnSynchronizationStart();
            
            lastSelectedIndex = Navigation.SelectedIndex;

            ShowHud(I18n.SynchronizationInProgress);
        }

        protected override void OnSynchronizationFinished()
        {
            base.OnSynchronizationFinished();

            HideHud();
            
            profileElement.RefreshUserData();
            
            SelectTab(lastSelectedIndex);
        }
        
        void SelectTab(int row)
        {   
            Navigation.SelectedIndex = row;
            
            Navigation.NavigationRoot.TableView.SelectRow(NSIndexPath.FromRowSection(row, 0),false,UITableViewScrollPosition.None);
        }
        
        #endregion
    }
}

