using System.Drawing;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.Dialog;

namespace BetterSalesman.iOS
{
    public class FlayoutNavigationItem : ImageStringElement, IFlyoutNavigationItem
    {   
        public string Controller { get; set; }
        public NSAction Callback { get; set; }
        UIImage Image;
        UIImageView cellImageView;     
        
        UIColor tintColor = AppDelegate.ColorTextGray;
        UIColor tintColorHightlighted = UIColor.White;

        public FlayoutNavigationItem(string caption, NSAction tapped, UIImage image, string controller) : base(caption, tapped, image)
        {
            Controller = controller;
            Image = image;
            Callback = tapped;
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
            
            cellImageView = cell.ImageView;
            
            cell.ImageView.TintColor = tintColor;
            
            var selectedView = new UIView { BackgroundColor = AppDelegate.ColorBackgroundOrange };
            cell.SelectedBackgroundView = selectedView;
            cell.BackgroundColor = AppDelegate.ColorBackgroundGray;
            
            cell.TextLabel.Font = UIFont.FromName("HelveticaNeue", 12);
            cell.TextLabel.TextColor = tintColor;
            cell.TextLabel.HighlightedTextColor = tintColorHightlighted;
            
            return cell;
        }

        public void Highlighted()
        {
            if (Image != null)
            {
                cellImageView.TintColor = tintColorHightlighted;
                
                cellImageView.HighlightedImage = Image.ImageWithRenderingMode(UIImageRenderingMode.AlwaysTemplate);
            }
        }
        
        public float GetHeight()
        {
            return 45;
        }
        
    }
}


