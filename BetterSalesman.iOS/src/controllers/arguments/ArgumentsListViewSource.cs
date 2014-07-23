using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BetterSalesman.Core.BusinessLayer;
using System.Collections.Generic;

namespace BetterSalesman.iOS
{
    public class ArgumentsListViewSource : UITableViewSource
    {
        public Dictionary<int, List<Argument>> Items = new Dictionary<int, List<Argument>>();
        
        readonly string cellIdentifierItem = "ArgumentsCell";
        readonly string cellIdentifierHeader = "ArgumentsHeader";
        
        string[] sections = {
            I18n.WithoutRating,
            I18n.Rated
        };

        public ArgumentsListViewSource()
        {
        }

        public override int NumberOfSections(UITableView tableView)
        {
            return Items.Count;
        }

        public override string TitleForHeader(UITableView tableView, int section)
        {
            if (Items.Count > 1) // not ordered & ordered
            {
                return sections[section];
            }
            
            return sections[1];
        }

        public override UIView GetViewForHeader(UITableView tableView, int section)
        {   
            var cell = tableView.DequeueReusableCell(cellIdentifierHeader) ?? new UITableViewCell();
            
            var titleTxt = (UILabel)cell.ViewWithTag(1);
            var imageView = (UIImageView)cell.ViewWithTag(2);
            
            titleTxt.Text = TitleForHeader(tableView, section);
            
            return cell;
        }

        public override float GetHeightForHeader(UITableView tableView, int section)
        {
            return 45;
        }

        public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 100;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            return Items[section].Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(cellIdentifierItem) ?? new UITableViewCell();
            
            var featureTxt = (UILabel)cell.ViewWithTag(1);
            var benefitTxt = (UILabel)cell.ViewWithTag(2);
            
            var argument = Items[indexPath.Section][indexPath.Row];
            
            featureTxt.Text = argument.Feature;
            benefitTxt.Text = argument.Benefit;
            
            return cell;
        }
    }
}

