using System;

namespace BetterSalesman.Core.ServiceAccessLayer
{
	public interface ILocalizationProvider
	{
		string GetLocalizedText(string localizationKey);
	}
}

