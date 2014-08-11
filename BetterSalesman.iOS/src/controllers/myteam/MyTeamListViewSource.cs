using MonoTouch.Foundation;
using MonoTouch.UIKit;
using SDWebImage;
using BetterSalesman.Core.BusinessLayer;
using System.Collections.Generic;

namespace BetterSalesman.iOS
{
    public class MyTeamListViewSource : UITableViewSource
    {
        readonly string cellIdentifierItem = "MyTeamCell";
        const string ic_placeholder = "avatar_placeholder";
        
        List<User> items = new List<User>();
        
        public MyTeamListViewSource(List<User> items)
        {
            this.items = items;
        }

        public override int NumberOfSections(UITableView tableView)
        {
            return 1;
        }
        
        public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 73;
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            return items.Count;
        }

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var cell = tableView.DequeueReusableCell(cellIdentifierItem) ?? new UITableViewCell();
            
            var displayName = (UILabel)cell.ViewWithTag(1);
            var experience = (UILabel)cell.ViewWithTag(2);
            var avatar = (UIImageView)cell.ViewWithTag(5);
            
            var user = items[indexPath.Row];
            
            displayName.Text = user.DisplayName;
            experience.Text = user.Experience + " " + I18n.XP;
            
			if (user != null && !string.IsNullOrEmpty(user.AvatarThumbUrl))
			{
	            avatar.SetImage(
	                url: new NSUrl(user.AvatarThumbUrl),
	                placeholder: UIImage.FromBundle(ic_placeholder)
	            );
			}
			else
			{
				avatar.Image = UIImage.FromBundle(ic_placeholder);
			}
            
            return cell;
        }
    }
}

