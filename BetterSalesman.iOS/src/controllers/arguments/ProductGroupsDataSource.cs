using System;
using BetterSalesman.Core.BusinessLayer;
using System.Collections.Generic;
using System.Threading.Tasks;
using BetterSalesman.Core.BusinessLayer.Managers;
using System.Linq;
using System.Diagnostics;

namespace BetterSalesman.iOS
{
	public delegate void SelectedProductGroupChanged(ProductGroup newSelectedProductGroup);

	public class ProductGroupsDataSource
	{
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

			#if DEBUG
			Debug.WriteLine("DEBUG MODE...");
			if (ProductGroups == null || ProductGroups.Count <= 0)
			{
				ProductGroups = new List<ProductGroup>
				{
					new ProductGroup
					{
						Name = "Test group #1"
					},
					new ProductGroup
					{
						Name = "Test group #2"
					},
					new ProductGroup
					{
						Name = "Test group #3"
					}
				};
			}
			#endif

			SelectedProductGroup = ProductGroups.FirstOrDefault();
		}

		private void OnSelectedProductGroupChanged(ProductGroup newSelectedProductGroup)
		{
			if (SelectedProductGroupChanged != null)
			{
				SelectedProductGroupChanged(newSelectedProductGroup);
			}
		}
	}
}

