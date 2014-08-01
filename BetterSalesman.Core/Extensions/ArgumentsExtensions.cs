using System.Collections.Generic;
using BetterSalesman.Core.BusinessLayer;
using System.Linq;

namespace BetterSalesman.Core.Extensions
{
    public static class ArgumentsExtensions
    {
        public static List<Argument> NotRated(this List<Argument> arguments)
        {
            return arguments.Where(a => !a.Rated).ToList();
        }

        public static List<Argument> Rated(this List<Argument> arguments)
        {
            return arguments.Where(a => a.Rated).ToList();
        }
    }
}

