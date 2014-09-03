using BetterSalesman.Core.BusinessLayer;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetterSalesman.Core.BusinessLayer.Managers;
using System.Linq;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.iOS
{
	public delegate void SelectedProductGroupChanged(ProductGroup newSelectedProductGroup);
	public delegate void ProductGroupPicked(ProductGroup pickedProductGroup);
	public delegate void ProductGroupsReloaded();

	public class ProductGroupsDataSource : UITableViewSource
	{
		private const string cellIdentifierItem = "cellProductGroup";

		public event SelectedProductGroupChanged SelectedProductGroupChanged;
		public event ProductGroupPicked ProductGroupPicked;
		public event ProductGroupsReloaded ProductGroupsReloaded;

		private ProductGroup selectedProductGroup;

		public ProductGroup SelectedProductGroup
		{
			get
			{
				return this.selectedProductGroup;
			}

			set
			{
				this.selectedProductGroup = value;
				OnSelectedProductGroupChanged(selectedProductGroup);
			}
		}
			
		public List<ProductGroup> ProductGroups { get; set; }

		public ProductGroupsDataSource() : base()
		{
			ProductGroups = new List<ProductGroup>();

			SynchronizationManager.Instance.FinishedSynchronization += isBackgroundSynchronization => ReloadProductGroups();
		}

		public void ReloadProductGroups()
		{
			ProductGroups = ProductGroupManager.GetProductGroups();
			OnProductGroupsReloaded();
			if (SelectedProductGroup == null || ProductGroups.All(pg => pg.Id != SelectedProductGroup.Id))
			{
				SelectedProductGroup = ProductGroups.FirstOrDefault();
			}
		}

		private void OnSelectedProductGroupChanged(ProductGroup newSelectedProductGroup)
		{
			if (SelectedProductGroupChanged != null)
			{
				SelectedProductGroupChanged(newSelectedProductGroup);
			}
		}

		public override int RowsInSection(UITableView tableview, int section)
		{
			return ProductGroups.Count;
		}
        
        public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
        {
            return 66;
        }

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell(cellIdentifierItem) ?? new UITableViewCell();

            var productGroupTxt = cell.ViewWithTag(1) as UILabel;
            var cellBackground = cell.ViewWithTag(2);

			var productGroup = ProductGroups[indexPath.Row];

			cellBackground.BackgroundColor = UIColor.Clear.FromHex(productGroup.ColorHex);
			productGroupTxt.Text = productGroup.Name;

			return cell;
		}
        
        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            var unratedArgumentIcon = cell.ViewWithTag(5) as UIImageView;
            var unratedArgumentLabel = cell.ViewWithTag(10) as UILabel;
            
            unratedArgumentIcon.Alpha = 0;
            unratedArgumentLabel.Alpha = 0;
            
            Task.Run(() => {
                ProductGroupManager.GetUnratedArgumentsCount(
                    ProductGroups[indexPath.Row],
                    UserManager.LoggedInUser().Id,
                    unratedArgumentsCount => 
                        InvokeOnMainThread(() =>
                        {
                            if ( unratedArgumentsCount > 0 )
                            {
                                unratedArgumentLabel.Text = unratedArgumentsCount.ToString();
                                UIView.BeginAnimations("cellAnimation");
                                UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
                                UIView.SetAnimationDuration(1);
                                unratedArgumentIcon.Alpha = 1;
                                unratedArgumentLabel.Alpha = 1;
                                UIView.CommitAnimations();
                            }
                        })
                );
            });
        }

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			this.SelectedProductGroup = this.ProductGroups[indexPath.Row];
			OnProductGroupPicked(this.SelectedProductGroup);
		}

		private void OnProductGroupPicked(ProductGroup pickedProductGroup)
		{
			if (ProductGroupPicked != null)
			{
				ProductGroupPicked(pickedProductGroup);
			}
		}

		private void OnProductGroupsReloaded()
		{
			if (ProductGroupsReloaded != null)
			{
				ProductGroupsReloaded();
			}
		}
	}
}

