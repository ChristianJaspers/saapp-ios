using System;
using BetterSalesman.Core.ServiceAccessLayer;

namespace BetterSalesman.iOS
{
	public class IosLocalizationProvider : ILocalizationProvider
	{
		public string GetLocalizedText(string localizationKey)
		{
			return localizationKey.t();
		}
	}
}

