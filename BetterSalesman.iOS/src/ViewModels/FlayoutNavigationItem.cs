using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.Dialog;

namespace BetterSalesman.iOS
{
    public class FlayoutNavigationItem : ImageStringElement
    {
        public string Controller;

        public FlayoutNavigationItem(string caption, NSAction tapped, UIImage image, string controller) : base(caption, tapped, image)
        {
            Controller = controller;
            // TODO add some extra stuff here for custom cell of this type
        }

        public override UITableViewCell GetCell(UITableView tv)
        {
            // TODO here we will customize cell 
            UITableViewCell cell = base.GetCell(tv);

            return cell;
        }
    }
}


