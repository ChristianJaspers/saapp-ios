using System;
using System.Collections.Generic;
using System.Diagnostics;
using MonoTouch.UIKit;
using BetterSalesman.Core.BusinessLayer.Managers;
using BetterSalesman.Core.ServiceAccessLayer;
using BetterSalesman.Core.BusinessLayer;

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
            
            Title = I18n.Arguments;
            
            var menuButton = new UIBarButtonItem(UIImage.FromBundle(menu_icon), UIBarButtonItemStyle.Plain, delegate
                {
                    FlyoutViewController.Navigation.ToggleMenu();
                });
            
            NavigationItem.SetLeftBarButtonItem(menuButton, false);
            
            TableView.Source = new ArgumentsListViewSource();
            
            LoadArguments();
            
            ServiceProviderArgument.Instance.Arguments(
                result => LoadArguments(),
                errorCode => Debug.WriteLine("Error during arguments fetching " + errorCode)
            );
        }

        void LoadArguments()
        {
            InvokeOnMainThread(() =>
                {
                    var allArguments = ArgumentManager.Arguments();
                    
                    ((ArgumentsListViewSource)TableView.Source).Items = new Dictionary<int, List<Argument>> {
                        { 0, allArguments },
                        { 1, allArguments }
                    };
        
                    TableView.ReloadData();
                });
        }
    }
}

