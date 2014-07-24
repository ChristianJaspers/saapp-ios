using System.Linq;
using System.Collections.Generic;
using BetterSalesman.Core.DataLayer;

namespace BetterSalesman.Core.BusinessLayer.Managers
{
    public static class ArgumentManager
    {        
        public static List<Argument> Arguments()
        {
            var items = new List<Argument>();
            
            using (var conn = DatabaseProvider.OpenConnection())
            {
                items = conn.Table<Argument>().ToList().OrderBy(a=>a.Relevance).ThenBy(a=>a.CreatedAt).ToList();
            }
            
            return items;
        }
    }
}

