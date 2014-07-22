using System;
using System.Collections.Generic;

namespace BetterSalesman.Core.ServiceAccessLayer
{
	public static class ErrorDescriptionProvider
	{
		public const int HostUnreachableErrorCode = 2000;
		public const int NetworkUnavailableErrorCode = 3000;
		public const int UnknownNetworkErrorCode = -1001;
		public const int FileNotFoundErrorCode = -1000;

		public static string GetMessageForCode(int errorCode)
		{
			if (!ErrorCodesToLocalizationKeys.ContainsKey(errorCode))
			{
				throw new ArgumentException("There is no error message for error code '" + errorCode + "'. Did you forget to add it to ErrorCodesLocalizationKeys dictionary?");
			}

			return ErrorCodesToLocalizationKeys[errorCode];
		}

		// TODO - add to Localizable.strings
		private static Dictionary<int, string> ErrorCodesToLocalizationKeys = new Dictionary<int, string> () {
			{ 2000, "Can't connect to server. Try again later." },
			{ 3000, "Make sure you're connected to Internet and try again." },
			{ -1001, "An unknown error occured. Please try again later." },
			{ -1000, "Couldn't find file you selected. Please select another file." }
		};
	}
}

