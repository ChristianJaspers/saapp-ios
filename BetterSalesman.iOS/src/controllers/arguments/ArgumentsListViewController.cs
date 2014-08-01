using System;
using System.Linq;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using BetterSalesman.Core.BusinessLayer.Managers;
using BetterSalesman.Core.BusinessLayer;
using BetterSalesman.Core.Extensions;
using System.Diagnostics;

namespace BetterSalesman.iOS
{
    public partial class ArgumentsListViewController : UITableViewController
    {   
        const string segueIdAdding = "ArgumentAdding";
        const string segueIdSelected = "ArgumentSelected";

		public ProductGroupsDataSource ProductGroupsDataSource { get; set; }

        public ArgumentsListViewController()
            : base(UITableViewStyle.Grouped)
        {
        }

        public ArgumentsListViewController(IntPtr handle)
            : base(handle)
        {
        }
        
        #region Lifecycle

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            
            Title = I18n.Arguments;
            
            TableView.Source = new ArgumentsListViewSource();
        }
        
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            
            LoadArguments();
        }
        
        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            
            if (segue.Identifier.Equals(segueIdSelected))
            {
                var cell = (UITableViewCell)sender;
                
                var indexPath = TableView.IndexPathForCell(cell);
                
                var vc = (ArgumentsDetail)segue.DestinationViewController;
                
                vc.Argument = ((ArgumentsListViewSource)TableView.Source).Items[indexPath.Section][indexPath.Row];
            }
        }
        
        #endregion

        void LoadArguments()
        {
            InvokeOnMainThread(() =>
            {
				var allArguments = ArgumentManager.Arguments();
				if (ProductGroupsDataSource != null && ProductGroupsDataSource.SelectedProductGroup != null)
				{
					Debug.WriteLine("Filtering arguments by ProductGroup: " + ProductGroupsDataSource.SelectedProductGroup.Name);
					allArguments = allArguments.Where(a => a.ProductGroupId == ProductGroupsDataSource.SelectedProductGroup.Id).ToList();
				}
                var notRatedArguments = allArguments.NotRated();
                var RatedArguments = allArguments.Rated();
                
                var items = new Dictionary<int, List<Argument>>();
                
                if ( notRatedArguments.Any() )
                {
                    items.Add(0,notRatedArguments);
                    items.Add(1,RatedArguments);
                }
                else
                {
                    items.Add(0,RatedArguments);
                }
                
                ((ArgumentsListViewSource)TableView.Source).Items = items;
    
                TableView.ReloadData();
            });
        }
    }
}

