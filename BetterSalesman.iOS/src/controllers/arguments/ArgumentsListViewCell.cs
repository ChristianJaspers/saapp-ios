using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace BetterSalesman
{
    public class ArgumentsListViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("ArgumentsCell");

        public ArgumentsListViewCell() : base(UITableViewCellStyle.Value1, Key)
        {
            // TODO: add subviews to the ContentView, set various colors, etc.
            TextLabel.Text = "TextLabel";
        }
    }
}

