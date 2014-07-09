using MonoTouch.UIKit;
using System;

namespace BetterSalesman
{
    public partial class ArgumentsListViewController : UITableViewController
    {
        public ArgumentsListViewController() : base(UITableViewStyle.Grouped)
        {
        }
        
        public ArgumentsListViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            TableView.Source = new ArgumentsListViewSource();
        }
    }
}

