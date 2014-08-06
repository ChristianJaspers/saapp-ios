using System;
using System.Linq;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using BetterSalesman.Core.BusinessLayer.Managers;
using BetterSalesman.Core.Extensions;
using System.Diagnostics;

namespace BetterSalesman.iOS
{
    public partial class ArgumentsListViewController : BaseUITableViewController
    {   
        const string segueIdAdding = "ArgumentAdding";
        const string segueIdSelected = "ArgumentSelected";
        
        const string ic_rated = "ic_star_rated";
        const string ic_unrated = "ic_star_unreated";

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
                
                vc.Argument = ((ArgumentsListViewSource)TableView.Source).Sections[indexPath.Section].Arguments[indexPath.Row];
            }
        }
        
        #endregion

        void LoadArguments()
        {
            var allArguments = ArgumentManager.Arguments();
            if (ProductGroupsDataSource != null && ProductGroupsDataSource.SelectedProductGroup != null)
            {
                Debug.WriteLine("Filtering arguments by ProductGroup: " + ProductGroupsDataSource.SelectedProductGroup.Name);
                allArguments = allArguments.Where(a => a.ProductGroupId == ProductGroupsDataSource.SelectedProductGroup.Id).ToList();
            }
            
            var notRatedArguments = allArguments.NotRated();
            var ratedArguments = allArguments.Rated();
            
            var notRatedArgumentsSection = new ListSectionArguments { Arguments = notRatedArguments, Title = I18n.WithoutRating, Icon = ic_unrated };
            var RatedArgumentsSection = new ListSectionArguments { Arguments = ratedArguments, Title = I18n.Rated, Icon = ic_rated };
            
            var items = new Dictionary<int, ListSectionArguments>();
            
            int i = 0;
            if ( notRatedArguments.Any() )
            {
                items.Add(i,notRatedArgumentsSection);
                i++;
            }
            if (ratedArguments.Any())
            {
                items.Add(i,RatedArgumentsSection);
            }
            
            InvokeOnMainThread(() =>
            {
                ((ArgumentsListViewSource)TableView.Source).Sections = items;
    
                TableView.ReloadData();
            });
        }
        
        protected override void OnSynchronizationFinished()
        {
            base.OnSynchronizationFinished();
            
            LoadArguments();
        }
    }
}

