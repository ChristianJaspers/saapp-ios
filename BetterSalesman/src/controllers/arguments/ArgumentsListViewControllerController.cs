﻿
using MonoTouch.UIKit;

namespace BetterSalesman
{
    public class ArgumentsListViewControllerController : UITableViewController
    {
        public ArgumentsListViewControllerController() : base(UITableViewStyle.Grouped)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
            
            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            // Register the TableView's data source
            TableView.Source = new ArgumentsListViewControllerSource();
        }
    }
}

