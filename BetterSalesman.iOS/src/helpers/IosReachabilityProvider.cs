using System;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.iOS
{
	public class IosReachabilityProvider : IReachabilityProvider
	{
		public bool IsHostReachable(string host)
		{
			return Reachability.IsHostReachable(host);
		}
	}
}

