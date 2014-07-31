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
				return selectedProductGroup;
			}

			set
			{
				if (selectedProductGroup != value)
				{
					selectedProductGroup = value;
					OnSelectedProductGroupChanged(selectedProductGroup);
				}
			}
		}
			
		public List<ProductGroup> ProductGroups { get; set; }

		public ProductGroupsDataSource() : base()
		{
			ProductGroups = new List<ProductGroup>();
		}

		public void Initialize()
		{
			ProductGroups = ProductGroupManager.GetProductGroups();

			SelectedProductGroup = ProductGroups.FirstOrDefault();
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

			var featureTxt = (UILabel)cell.ViewWithTag(1);
			var benefitTxt = (UILabel)cell.ViewWithTag(2);

			var relevanceTxt = (UILabel)cell.ViewWithTag(5);
			var verticalLine = (UIView)cell.ViewWithTag(6);

			var productGroup = ProductGroups[indexPath.Row];

			return cell;
		}
	}
}

