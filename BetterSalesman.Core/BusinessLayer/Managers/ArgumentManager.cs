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
                // some order here?
                items = conn.Table<Argument>().ToList();
            }
            
            return items;
        }
    }
}

