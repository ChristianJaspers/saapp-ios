using System;
using MonoTouch.UIKit;
using FlyoutNavigation;
using System.Collections.Generic;
using MonoTouch.Dialog;

namespace BetterSalesman
{
    public partial class RootViewController : UIViewController
    {
        public FlyoutNavigationController Navigation;
        
        static bool UserInterfaceIdiomIsPhone
        {
            get { return UIDevice.CurrentDevice.UserInterfaceIdiom == UIUserInterfaceIdiom.Phone; }
        }

        public RootViewController(IntPtr handle) : base(handle)
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
            
            Navigation.ShowMenu();
        }

        #endregion
        
        #region Flayout Navigation
        
        void NavigationLayoutInit()
        {
            Navigation = new FlyoutNavigationController();
            Navigation.View.Frame = UIScreen.MainScreen.Bounds;
            Navigation.AlwaysShowLandscapeMenu = false;
            View.AddSubview(Navigation.View);

            PopulateNavigationItems();
        }

        void PopulateNavigationItems()
        {
            var elements = new List<FlayoutNavigationItem>();

            elements.Add(
                new FlayoutNavigationItem(
                    "Lorem ipsum", 
                    () => { /* TODO action here */ },
                    UIImage.FromBundle(""), // TODO icons here
                    "controller"
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
        
        UIViewController controllerForSection(string title)
        {
            BaseUINavigationController vc = (BaseUINavigationController)Storyboard.InstantiateViewController("NavigationBase");
            vc.InitialControllerType = title;
            return vc;
        }
        
        #endregion
    }
}

