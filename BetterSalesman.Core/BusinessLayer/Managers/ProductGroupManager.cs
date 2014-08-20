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
			var items = new List<ProductGroup>();

			using (var connection = DatabaseProvider.OpenConnection())
			{
				items = connection.Table<ProductGroup>().ToList();
			}

			return items;
		}
        
        public static ProductGroup GetProductGroup(int pk)
        {
            ProductGroup pg = null;
            using (var connection = DatabaseProvider.OpenConnection())
            {
                pg = connection.Get<ProductGroup>(pk);
            }
            return pg;
        }
	}
}

