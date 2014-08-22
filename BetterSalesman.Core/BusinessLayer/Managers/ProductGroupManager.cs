using System;
using System.Collections.Generic;
using BetterSalesman.Core.DataLayer;
using System.Linq;

namespace BetterSalesman.Core.BusinessLayer.Managers
{
	public class ProductGroupManager
	{
		public ProductGroupManager()
		{
		}

		public static List<ProductGroup> GetProductGroups()
		{
			return DatabaseHelper.GetAll<ProductGroup>().ToList();
		}
        
        public static ProductGroup GetProductGroup(int id)
        {
			return DatabaseHelper.Get<ProductGroup>(id);
        }
	}
}

