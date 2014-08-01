using System;
using BetterSalesman.Core.BusinessLayer;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetterSalesman.Core.BusinessLayer.Managers;
using System.Linq;
using System.Diagnostics;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace BetterSalesman.iOS
{
	public delegate void SelectedProductGroupChanged(ProductGroup newSelectedProductGroup);

	public class ProductGroupsDataSource : UITableViewSource
	{
		private const string cellIdentifierItem = "cellProductGroup";

		public event SelectedProductGroupChanged SelectedProductGroupChanged;

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

		private bool hasBeenInitialized;

		public ProductGroupsDataSource() : base()
		{
			ProductGroups = new List<ProductGroup>();
		}

		public void Initialize()
		{
			ProductGroups = ProductGroupManager.GetProductGroups();

			if (!hasBeenInitialized)
			{
				SelectedProductGroup = ProductGroups.FirstOrDefault();
				hasBeenInitialized = true;
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
		}
	}
}

