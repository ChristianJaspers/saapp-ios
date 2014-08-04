using System;

using MonoTouch.UIKit;
using BetterSalesman.Core.BusinessLayer.Managers;

namespace BetterSalesman.iOS
{
    public partial class MyTeamListViewController : BaseUITableViewController
	{
        const string menu_icon = "ic_menu";
        
		public MyTeamListViewController (IntPtr handle) : base (handle)
		{
		}
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            LoadUsers();
            
            Title = I18n.MyTeam;
            
            var menuButton = new UIBarButtonItem(UIImage.FromBundle(menu_icon), UIBarButtonItemStyle.Plain, delegate
            {
                FlyoutViewController.Navigation.ToggleMenu();
            });

            NavigationItem.SetLeftBarButtonItem(menuButton, true);
        }

        void LoadUsers()
        {
            var users = UserManager.Users();
            
            TableView.Source = new MyTeamListViewSource(users);
            
            InvokeOnMainThread(TableView.ReloadData);
        }
        
        protected override void OnSynchronizationFinished()
        {
            base.OnSynchronizationFinished();

            LoadUsers();
        }
	}
}
