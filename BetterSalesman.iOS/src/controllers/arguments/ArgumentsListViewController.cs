using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using MonoTouch.UIKit;
using BetterSalesman.Core.BusinessLayer.Managers;
using BetterSalesman.Core.ServiceAccessLayer;
using BetterSalesman.Core.BusinessLayer;
using BetterSalesman.Core.Extensions;

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
                var notRatedArguments = allArguments.Grouped();
                var RatedArguments = allArguments.NotGrouped();
                
                var items = new Dictionary<int, List<Argument>>();
                
                if ( notRatedArguments.Any() )
                {
                    items.Add(0,notRatedArguments);
                    items.Add(1,RatedArguments);
                }
                else
                {
                    items.Add(0,RatedArguments);
                }
                
                ((ArgumentsListViewSource)TableView.Source).Items = items;
    
                TableView.ReloadData();
            });
        }
    }
}

