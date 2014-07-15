using MonoTouch.Foundation;

namespace BetterSalesman.iOS
{
	public static class LocalizationExtensions
	{
		public static string t(this string translate)
		{
			return NSBundle.MainBundle.LocalizedString(translate, "", "");
		}
	}
}

