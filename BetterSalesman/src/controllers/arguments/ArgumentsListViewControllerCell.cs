using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace BetterSalesman
{
    public class ArgumentsListViewControllerCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("ArgumentsListViewControllerCell");

        public ArgumentsListViewControllerCell() : base(UITableViewCellStyle.Value1, Key)
        {
            // TODO: add subviews to the ContentView, set various colors, etc.
            TextLabel.Text = "TextLabel";
        }
    }
}

