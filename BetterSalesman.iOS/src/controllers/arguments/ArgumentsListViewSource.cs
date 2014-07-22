using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BetterSalesman.Core.BusinessLayer;
using System.Collections.Generic;

namespace BetterSalesman.iOS
{
    public class ArgumentsListViewSource : UITableViewSource
    {
        readonly string cellIdentifierItem = "ArgumentsCell";
        public List<Argument> items = new List<Argument>();
        public ArgumentsListViewSource()
        {
        }

        public override int NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            return items.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(cellIdentifierItem) ?? new UITableViewCell();
            
            var featureTxt = (UILabel)cell.ViewWithTag(1);
            var benefitTxt = (UILabel)cell.ViewWithTag(2);
            
            var argument = items[indexPath.Row];
            
            featureTxt.Text = argument.Feature;
            benefitTxt.Text = argument.Benefit;
            
            return cell;
        }
    }
}

