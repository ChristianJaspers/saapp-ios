using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace BetterSalesman
{
    public class ArgumentsListViewControllerSource : UITableViewSource
    {
        public ArgumentsListViewControllerSource()
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
            return 1;
        }

        public override string TitleForHeader(UITableView tableView, int section)
        {
            return "Header";
        }

        public override string TitleForFooter(UITableView tableView, int section)
        {
            return "Footer";
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(ArgumentsListViewControllerCell.Key) as ArgumentsListViewControllerCell;
            if (cell == null)
                cell = new ArgumentsListViewControllerCell();
            
            // TODO: populate the cell with the appropriate data based on the indexPath
            cell.DetailTextLabel.Text = "DetailsTextLabel";
            
            return cell;
        }
    }
}

