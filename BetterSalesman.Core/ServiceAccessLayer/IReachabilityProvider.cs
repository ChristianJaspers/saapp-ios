using System;

namespace BetterSalesman.Core.ServiceAccessLayer
{
	public interface IReachabilityProvider
	{
		bool IsHostReachable(string host);
	}
}

