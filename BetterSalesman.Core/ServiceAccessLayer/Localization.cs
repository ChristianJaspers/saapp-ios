using System;
using System.Diagnostics;

namespace BetterSalesman.Core.ServiceAccessLayer
{
	public class Localization
	{
		private static Localization instance;
		private static object locker = new object();

		private ILocalizationProvider localizationProvider;

		public static Localization Instance
		{
			get
			{
				if (instance == null)
				{
					lock(locker)
					{
						if (instance == null)
						{
							instance = new Localization();
						}
					}
				}

				return instance;
			}
		}

		private Localization() : base()
		{
		}

		public void Initialize(ILocalizationProvider provider)
		{
			localizationProvider = provider;
		}

		public string GetLocalizedText(string localizationKey)
		{
			var managerClassName = this.GetType().Name;
			var localizationProviderInterfaceName = typeof(ILocalizationProvider).Name;
			var uninitializedErrorMessage = string.Format("{0} requires platform specific {1} instance to be provided before it's used. Did you forget to call Initialize method?", managerClassName, localizationProviderInterfaceName);
			Debug.Assert(localizationProvider != null, uninitializedErrorMessage);

			return localizationProvider.GetLocalizedText(localizationKey);
		}
	}
}

