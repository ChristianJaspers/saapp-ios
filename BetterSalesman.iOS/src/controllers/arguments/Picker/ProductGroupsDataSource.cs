using System;
using BetterSalesman.Core.BusinessLayer;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetterSalesman.Core.BusinessLayer.Managers;
using System.Linq;
using System.Diagnostics;
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

			SynchronizationManager.Instance.FinishedSynchronization += (bool isBackgroundSynchronization) => 
				{
					ReloadProductGroups();
				};
		}

		public void ReloadProductGroups()
		{
			ProductGroups = ProductGroupManager.GetProductGroups();
			OnProductGroupsReloaded();
			if (SelectedProductGroup == null || !ProductGroups.Where(pg => pg.Id == SelectedProductGroup.Id).Any())
			{
				Debug.WriteLine("Selected argument changed");
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

			var productGroupTxt = (UILabel)cell.ViewWithTag(1);
			var cellBackground = cell.ViewWithTag(2);

			var productGroup = ProductGroups[indexPath.Row];

			cellBackground.BackgroundColor = UIColor.Clear.FromHex(productGroup.ColorHex);
			productGroupTxt.Text = productGroup.Name;

			return cell;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			Debug.WriteLine("Selected ProductGroup: " + this.ProductGroups[indexPath.Row].Name);
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

