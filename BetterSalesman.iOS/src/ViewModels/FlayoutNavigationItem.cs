using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.Dialog;

namespace BetterSalesman.iOS
{
    public class FlayoutNavigationItem : ImageStringElement, IFlyoutNavigationItem
    {   
        public string Controller { get; set; }

        public FlayoutNavigationItem(string caption, NSAction tapped, UIImage image, string controller) : base(caption, tapped, image)
        {
            Controller = controller;
        }

        public override UITableViewCell GetCell(UITableView tv)
        {
            UITableViewCell cell = base.GetCell(tv);
            
            var line = cell.ViewWithTag(10);
            if (line == null)
            {
                line = new UIView { 
                    BackgroundColor = UIColor.Clear.FromHex("#BBBAC0"),
                    Frame = new RectangleF(0,0,cell.Frame.Size.Width,1),
                    Tag = 10
                };
                cell.Add(line);
            }
            
            cell.SelectedBackgroundView = new UIView { BackgroundColor = AppDelegate.ColorBackgroundOrange };
            cell.BackgroundColor = AppDelegate.ColorBackgroundGray;
            
            cell.TextLabel.Font = UIFont.FromName("HelveticaNeue", 12);
            cell.TextLabel.TextColor = AppDelegate.ColorTextGray;
            cell.TextLabel.HighlightedTextColor = UIColor.White;
            cell.TintColor = AppDelegate.ColorTextGray;

            return cell;
        }
    }
}


