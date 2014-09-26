using System;
using System.Diagnostics;

namespace BetterSalesman.Core.ServiceAccessLayer
{
	/// <summary>
	/// This class is responsible for supplying localized text in a cross-platform fashion.
	///
	/// NOTE! Since localization is done differently for each platform a ILocalizationProvider interface has to be implemented for each platform and passed during initialization.
	/// </summary>
	public class Localization
	{
		private static Localization instance;
		private static object locker = new object();

		private ILocalizationProvider localizationProvider;

		private string instanceNotIntitializedErrorMessage;

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
			var managerClassName = this.GetType().Name;
			var localizationProviderInterfaceName = typeof(ILocalizationProvider).Name;
			instanceNotIntitializedErrorMessage = string.Format("{0} requires platform specific {1} instance to be provided before it's used. Did you forget to call Initialize method?", managerClassName, localizationProviderInterfaceName);
		}

		public void Initialize(ILocalizationProvider provider)
		{
			localizationProvider = provider;
		}

		/// <summary>
		/// This method returns localized text.
		///
		/// NOTE! This method requires localization provider to have been intialized with ILocalizationProvider instance.
		/// </summary>
		/// <returns>The localized text for given localizationKey</returns>
		/// <param name="localizationKey">Localization key.</param>
		public string GetLocalizedText(string localizationKey)
		{
			Debug.Assert(localizationProvider != null, instanceNotIntitializedErrorMessage);

			return localizationProvider.GetLocalizedText(localizationKey);
		}
	}
}

