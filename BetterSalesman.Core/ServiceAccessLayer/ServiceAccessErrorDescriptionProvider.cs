using System;

namespace BetterSalesman.Core.ServiceAccessLayer
{
	// TODO - can't localize in core since localization is done per-platform - add localizetion provider?
	public class ServiceAccessErrorDescriptionProvider
	{
		// TODO - localize
		public static int HostUnreachableErrorCode = 2000;
		public static string HostUnreachableErrorMessage = "Can't connect to server. Try again later.";

		public static int NetworkUnavailableErrorCode = 3000;
		public static string NetworkUnavailableErrorMessage = "Make sure you're connected to Internet and try again.";

		public const int UnknownNetworkErrorCode = -1001;
		public const string UnknownNetworErrorMessage = "An unknown error occured. Please try again later.";

		public const int FileNotFoundErrorCode = -1000;
		public const string FileNotFoundErrorMessage = "Couldn't find file you selected. Please select another file.";
	}
}

