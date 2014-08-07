using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.Dialog;
using BetterSalesman.Core.BusinessLayer;
using BetterSalesman.Core.BusinessLayer.Managers;
using SDWebImage;

namespace BetterSalesman.iOS
{
    public class FlayoutNavigationItemProfile : UIViewElement, IFlyoutNavigationItem
    {
        public string Controller { get; set; }

        const string AvatarPlaceholderImageName = "avatar_placeholder.png";
        const string profileXib = "sidebarProfile";
        
        UIImage AvatarPlaceholderImage;
        UIImageView ProfileImageView;
        UILabel labelViewProfile;
        UILabel labelDisplayName;
        

        public FlayoutNavigationItemProfile(string caption, string controller, UIView view = null, bool transparent = true) : base(caption, view, transparent)
        {
            Controller = controller;
            
            NSArray views = NSBundle.MainBundle.LoadNib(profileXib, null, null);
            
            View = views.GetItem<UIView>(0);
            
            ProfileImageView = View.ViewWithTag(1) as UIImageView;
            labelDisplayName = View.ViewWithTag(2) as UILabel;
            labelViewProfile = View.ViewWithTag(3) as UILabel;
            
            labelViewProfile.Text = I18n.ViewProfile;
            
            AvatarPlaceholderImage = UIImage.FromBundle(AvatarPlaceholderImageName).Circle();
        }
        
        public void RefreshUserData()
        {
            var user = UserManager.LoggedInUser();
            
            labelDisplayName.Text = user.DisplayName;

            var downloadOptions = SDWebImageOptions.ProgressiveDownload 
                | SDWebImageOptions.ContinueInBackground
                | SDWebImageOptions.RetryFailed;

            ProfileImageView.SetImage(new NSUrl(user.AvatarUrl), AvatarPlaceholderImage, downloadOptions, ProcessImageDownloadCompleted);
        }
        
        private void ProcessImageDownloadCompleted(UIImage downloadedImage, NSError error, SDImageCacheType cacheType)
        {
            ProfileImageView.Image = downloadedImage.Circle();
        }
        
        public override UITableViewCell GetCell(UITableView tv)
        {
            var cell = base.GetCell(tv);
            
            var clearView = new UIView { BackgroundColor = UIColor.Clear };
            
            cell.BackgroundView = clearView;
            cell.SelectedBackgroundView = clearView;
            
            return cell;
        }
    }
}


