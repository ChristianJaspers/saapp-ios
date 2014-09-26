using System.Linq;
using System.Collections.Generic;
using BetterSalesman.Core.DataLayer;

namespace BetterSalesman.Core.BusinessLayer.Managers
{
    public static class ArgumentManager
    {        
        public static List<Argument> GetArguments()
        {
			return DatabaseHelper.GetAll<Argument>().OrderByDescending(a=>a.Rating).ThenBy(a=>a.CreatedAt).ToList();
        }
    }
}

