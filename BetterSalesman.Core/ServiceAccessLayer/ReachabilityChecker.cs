using System;
using System.Diagnostics;

namespace BetterSalesman.Core.ServiceAccessLayer
{
	/// <summary>
	/// This class is responsible for determining whether given network host is reachable.
	///
	/// NOTE! Since at the time of writing (2014-08-27), there is no cross-platform way of determining reachability that doesn't rely on 3rd party components, the platform-specific implementation must be supplied during intialization.
	/// </summary>
	public class ReachabilityChecker
	{
		private static ReachabilityChecker instance;
		private static object locker = new object();

		private IReachabilityProvider reachabilityProvider;

		private string instanceNotIntitializedErrorMessage;

		public static ReachabilityChecker Instance
		{
			get
			{
				if (instance == null)
				{
					lock(locker)
					{
						if (instance == null)
						{
							instance = new ReachabilityChecker();
						}
					}
				}

				return instance;
			}
		}

		public ReachabilityChecker() : base()
		{
			var managerClassName = this.GetType().Name;
			var localizationProviderInterfaceName = typeof(IReachabilityProvider).Name;
			instanceNotIntitializedErrorMessage = string.Format("{0} requires platform specific {1} instance to be provided before it's used. Did you forget to call Initialize method?", managerClassName, localizationProviderInterfaceName);
		}

		public void Initialize(IReachabilityProvider provider)
		{
			reachabilityProvider = provider;
		}

		public bool IsHostReachable(string host)
		{
			Debug.Assert(reachabilityProvider != null, instanceNotIntitializedErrorMessage);

			return reachabilityProvider.IsHostReachable(host);
		}
	}
}

