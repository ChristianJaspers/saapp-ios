using MonoTouch.UIKit;
using MonoTouch.Foundation;
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
            
            // Register the TableView's data source
            TableView.Source = new ArgumentsListViewSource();
        }
    }
}

