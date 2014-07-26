using System;

namespace BetterSalesman.Core.ServiceAccessLayer
{
	// TODO - can't localize in core since localization is done per-platform - add localizetion provider?
	public static class ErrorDescriptionProvider
	{
		// TODO - localize
		public static int HostUnreachableErrorCode = 2000;
		public static string HostUnreachableErrorMessage = "Can't connect to server. Try again later.";

		public static int NetworkUnavailableErrorCode = 3000;
		public static string NetworkUnavailableErrorMessage = "Couldn't find active Internet connection. Make sure you're connected to Internet and try again.";

		public const int UnknownNetworkErrorCode = -1001;
		public const string UnknownNetworErrorMessage = "An unknown error occured. Try again later.";

		public const int FileNotFoundErrorCode = -1000;
		public const string FileNotFoundErrorMessage = "Couldn't find the file you selected. Try a different one.";
	}
}

