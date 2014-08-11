using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.Dialog;
using System.Collections.Generic;

namespace BetterSalesman.iOS
{
    class UIFLyoutNavigationSource : UITableViewSource
	{
        List<Element> elements;
        
        public UIFLyoutNavigationSource(List<Element> elements)
        {
            this.elements = elements;
        }

        #region implemented abstract members of UITableViewSource

        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            var element = (IFlyoutNavigationItem)elements[indexPath.Row];
            
            return element.GetCell(tableView);
        }

        public override int RowsInSection(UITableView tableview, int section)
        {
            return 5;
        }
        
        public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            var element = (IFlyoutNavigationItem)elements[indexPath.Row];
            
            return element.GetHeight();
        }
        
        #endregion

        public override void RowHighlighted(UITableView tableView, NSIndexPath rowIndexPath)
        {
            if (rowIndexPath.Row > 0)
            {
                var element = (IFlyoutNavigationItem)elements[rowIndexPath.Row];
                
                element.Highlighted();
            }
        }
        
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var element = (IFlyoutNavigationItem)elements[indexPath.Row];
            
            if (element.Callback != null)
            {
                element.Callback();
                
            } 
            
            tableView.DeselectRow(indexPath, true);
            
            FlyoutViewController.Navigation.SelectedIndex = indexPath.Row;
            
            FlyoutViewController.Navigation.HideMenu();
        }
	}
}

