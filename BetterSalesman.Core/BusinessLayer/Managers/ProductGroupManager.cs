using System;
using System.Collections.Generic;
using BetterSalesman.Core.DataLayer;
using System.Linq;
using System.Diagnostics;

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
        
        public static void GetUnratedArgumentsCount(ProductGroup productGroup, int NotCountedArgumentsForUserId, Action<int> successCallback)
        {
            List<Argument> unratedArguments = new List<Argument>();
            lock (DatabaseProvider.databaseWriteLocker)
            {
                using (var conn = DatabaseProvider.OpenConnectionReadWrite())
                {
                    unratedArguments = conn.Table<Argument>()
                        .Where(
                            argument =>
                                argument.ProductGroupId == productGroup.Id &
                                argument.UserId != NotCountedArgumentsForUserId & 
                                argument.MyRating == 0
                        ).ToList();
                    
                    successCallback(unratedArguments.Count);
                }
            }
        }
	}
}

