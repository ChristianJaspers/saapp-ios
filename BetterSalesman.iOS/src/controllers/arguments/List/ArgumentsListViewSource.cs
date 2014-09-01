using MonoTouch.Foundation;
using MonoTouch.UIKit;
using BetterSalesman.Core.BusinessLayer;
using System.Collections.Generic;
using System.Drawing;

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
            
            var titleTxt = cell.ViewWithTag(1) as UILabel;
            var imageView = cell.ViewWithTag(2) as UIImageView;
            
            titleTxt.Text = Sections[section].Title;
            titleTxt.TextColor = AppDelegate.ColorTextDarkGray;
            
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
            
            var featureTxt = cell.ViewWithTag(1) as UILabel;
            var benefitTxt = cell.ViewWithTag(2) as UILabel;
            var relevanceTxt = cell.ViewWithTag(5) as UILabel;
            var verticalLine = cell.ViewWithTag(6);
            
            var argument = Sections[indexPath.Section].Arguments[indexPath.Row];
            
            var frame = verticalLine.Frame;
            frame.Width = 0.5f;
            verticalLine.Frame = frame;
            
            relevanceTxt.TextColor = AppDelegate.ColorOrange;
            
            featureTxt.Text = argument.Feature;
            benefitTxt.Text = argument.Benefit;
            
            relevanceTxt.Text = Sections[indexPath.Section].Title != I18n.WithoutRating ? argument.Rating.ToString() : "!";
            
            return cell;
        }
    }
}

