using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace BetterSalesman
{
    public class ArgumentsListViewSource : UITableViewSource
    {
        readonly string cellIdentifierItem = "ArgumentsCell";
        public ArgumentsListViewSource()
        {
        }

        public override int NumberOfSections(UITableView tableView)
        {
            // TODO: return the actual number of sections
            return 1;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            // TODO: return the actual number of items in the section
            return 3;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(cellIdentifierItem) ?? new UITableViewCell();
            
            var title = (UILabel)cell.ViewWithTag(1);
            var subtitle = (UILabel)cell.ViewWithTag(2);
            
            title.Text = "Feature";
            subtitle.Text = "Benefit";
            
            return cell;
        }
    }
}

