using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace BetterSalesman.iOS
{
    public interface IFlyoutNavigationItem
    {
        NSAction Callback { get; set; }

        string Controller { get; set; }
        
        UITableViewCell GetCell(UITableView tableView);

        float GetHeight();

        void Highlighted();
    }
}

