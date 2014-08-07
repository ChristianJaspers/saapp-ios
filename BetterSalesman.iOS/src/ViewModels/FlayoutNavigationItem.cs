using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.Dialog;

namespace BetterSalesman.iOS
{
    public class FlayoutNavigationItem : ImageStringElement, IFlyoutNavigationItem
    {   
        public string Controller { get; set; }
        UIImage Image;

        public FlayoutNavigationItem(string caption, NSAction tapped, UIImage image, string controller) : base(caption, tapped, image)
        {
            Controller = controller;
            Image = image;
        }

        public override UITableViewCell GetCell(UITableView tv)
        {
            UITableViewCell cell = base.GetCell(tv);
            
            cell.SelectionStyle = UITableViewCellSelectionStyle.Default;
            
            var line = cell.ViewWithTag(10);
            if (line == null)
            {
                line = new UIView { 
                    BackgroundColor = UIColor.Clear.FromHex("#BBBAC0"),
                    Frame = new RectangleF(0,0,cell.Frame.Size.Width,0.5f),
                    Tag = 10
                };
                cell.Add(line);
            }
            
            cell.ImageView.TintColor = AppDelegate.ColorBackgroundGray;
            
            // TODO image tinting
            cell.ImageView.Image = new UIImage();
//            cell.ImageView.Image = Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
//            cell.ImageView.TintColor = UIColor.White;
            
            var selectedView = new UIView { BackgroundColor = AppDelegate.ColorBackgroundOrange };
            cell.SelectedBackgroundView = selectedView;
            cell.BackgroundColor = AppDelegate.ColorBackgroundGray;
            
            cell.TextLabel.Font = UIFont.FromName("HelveticaNeue", 12);
            cell.TextLabel.TextColor = AppDelegate.ColorTextGray;
            cell.TextLabel.HighlightedTextColor = UIColor.White;
            
            return cell;
        }
    }
}


