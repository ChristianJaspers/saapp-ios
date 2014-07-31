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
        
        const string ic_rated = "ic_star_rated";
        const string ic_unrated = "ic_star_unreated";
        
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
            
            string star_icon;
            star_icon = ic_rated;
            if (Items.Count > 1)
            {
                star_icon = section == 1 ? ic_rated : ic_unrated;
            }
            
            imageView.Image = UIImage.FromBundle(star_icon);
            
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
            
            var relevanceTxt = (UILabel)cell.ViewWithTag(5);
            var verticalLine = (UIView)cell.ViewWithTag(6);
            
            var argument = Items[indexPath.Section][indexPath.Row];
            
            verticalLine.Hidden = !argument.Rated;
            relevanceTxt.Hidden = !argument.Rated;
            
            featureTxt.Text = argument.Feature;
            benefitTxt.Text = argument.Benefit;
            relevanceTxt.Text = argument.Rating.ToString();
            
            return cell;
        }
    }
}

