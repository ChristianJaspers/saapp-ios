using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;
using System.Diagnostics;
using BetterSalesman.Core.BusinessLayer;
using System.Collections.Generic;

namespace BetterSalesman.iOS
{
	partial class ProductGroupPickerListViewController : UITableViewController
	{
		public ProductGroupPickerListViewController(IntPtr handle) : base(handle)
		{
		}
        
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            TableView.DataSource = new ProductGroupsDataSource();
        }

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			base.PrepareForSegue(segue, sender);

			LogSegue(segue);
		}

		private void LogSegue(UIStoryboardSegue segue)
		{
			Debug.WriteLine(string.Format("Segue {0} from {1} to {2}", segue.Identifier, segue.SourceViewController.GetType().Name, segue.DestinationViewController.GetType().Name));
		}
        
        class ProductGroupsDataSource : UITableViewDataSource
        {
            public override int RowsInSection(UITableView tableview, int section)
            {
                return 5;
            }
            const string cellIdentifierItem = "cellProductGroup";
            
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(cellIdentifierItem) ?? new UITableViewCell();

                var productGroupTxt = (UILabel)cell.ViewWithTag(1);
                var cellBackground = cell.ViewWithTag(2);
                
                var productGroup = new ProductGroup {
                    ColorHex = "#14151a",
                    Name = "Product group name",
                };
                
                cellBackground.BackgroundColor = UIColor.Clear.FromHex(productGroup.ColorHex);
                productGroupTxt.Text = productGroup.Name;

                return cell;
            }
        }
	}
}
