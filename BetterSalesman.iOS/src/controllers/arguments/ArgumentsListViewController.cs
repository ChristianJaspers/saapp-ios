using System;
using System.Diagnostics;
using MonoTouch.UIKit;
using BetterSalesman.Core.BusinessLayer.Managers;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.iOS
{
    public partial class ArgumentsListViewController : UITableViewController
    {
        const string menu_icon = "ic_menu";

        public ArgumentsListViewController()
            : base(UITableViewStyle.Grouped)
        {
        }

        public ArgumentsListViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            TableView.Source = new ArgumentsListViewSource();
            
            var menuButton = new UIBarButtonItem(UIImage.FromBundle(menu_icon), UIBarButtonItemStyle.Plain, delegate
            {
                FlyoutViewController.Navigation.ToggleMenu();
            });

            NavigationItem.SetLeftBarButtonItem(menuButton, true);
            
            LoadArguments();
            
            ServiceProviderArgument.Instance.Arguments(
                result => LoadArguments(),
                async errorCode => Debug.WriteLine("Error during fetching " + errorCode)
            );
        }

        void LoadArguments()
        {
            InvokeOnMainThread(() =>
            {
                var items = ArgumentManager.Arguments();
                    
                ((ArgumentsListViewSource)TableView.Source).items = items;
        
                TableView.ReloadData();
            });
        }
    }
}

