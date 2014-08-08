using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BetterSalesman.Core.BusinessLayer;
using System.Collections.Generic;

namespace BetterSalesman.iOS
{
    public class ArgumentsListViewSource : UITableViewSource
    {
        public Dictionary<int, ListSectionArguments> Sections = new Dictionary<int, ListSectionArguments>();
        
        readonly string cellIdentifierItem = "ArgumentsCell";
        readonly string cellIdentifierHeader = "ArgumentsHeader";

        public ArgumentsListViewSource()
        {
        }

        public override int NumberOfSections(UITableView tableView)
        {
            return Sections.Count;
        }

        public override UIView GetViewForHeader(UITableView tableView, int section)
        {   
            var cell = tableView.DequeueReusableCell(cellIdentifierHeader) ?? new UITableViewCell();
            
            var titleTxt = (UILabel)cell.ViewWithTag(1);
            
            var imageView = (UIImageView)cell.ViewWithTag(2);
            
            titleTxt.Text = Sections[section].Title;
            
            imageView.Image = UIImage.FromBundle(Sections[section].Icon);
            
            return cell;
        }

        public override float GetHeightForHeader(UITableView tableView, int section)
        {
            return 30;
        }

        public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 70;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            return Sections[section].Arguments.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(cellIdentifierItem) ?? new UITableViewCell();
            
            var bg = cell.ViewWithTag(15);
            
            bg.BackgroundColor = indexPath.Row % 2 > 0 ? AppDelegate.ColorBackgroundZebraOdd : AppDelegate.ColorBackgroundZebraEven; 
            
            var featureTxt = (UILabel)cell.ViewWithTag(1);
            var benefitTxt = (UILabel)cell.ViewWithTag(2);
            
            var relevanceTxt = (UILabel)cell.ViewWithTag(5);
            var verticalLine = cell.ViewWithTag(6);
            
            var argument = Sections[indexPath.Section].Arguments[indexPath.Row];
            
            verticalLine.Hidden = !argument.Rated;
            relevanceTxt.Hidden = !argument.Rated;
            
            featureTxt.Text = argument.Feature;
            benefitTxt.Text = argument.Benefit;
            relevanceTxt.Text = argument.Rating.ToString();
            
            return cell;
        }
    }
}

