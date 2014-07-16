using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SDWebImage;
using System;

namespace BetterSalesman.iOS
{
    public class MyTeamListViewSource : UITableViewSource
    {
        readonly string cellIdentifierItem = "MyTeamCell";
        public MyTeamListViewSource()
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
            var imageView = (UIImageView)cell.ViewWithTag(5);
            
            // TODO change values later on
            title.Text = "Username";
            subtitle.Text = "99999 XP";
            imageView.SetImage(
                url: new NSUrl("http://lorempixel.com/400/400/?t=" + new Random().Next(99999)),
                placeholder:  UIImage.FromBundle("ic_menu")
            );
            
            return cell;
        }
    }
}

