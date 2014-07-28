using System;
using Newtonsoft.Json;

namespace BetterSalesman.Core.ServiceAccessLayer
{
	public class ServiceAccessError
	{
		// TODO - localize with LocalizationProvider and ILocalicationProvider implemented per platform and remove ErrorDescriptionProvider
		public static ServiceAccessError ErrorUnknown = new ServiceAccessError(-1000, "Something went wrong. Please try again later.");
		public static ServiceAccessError ErrorFileNotFound = new ServiceAccessError(-1001, "Couldn't find the requested file.");
		public static ServiceAccessError ErrorHostUnreachable = new ServiceAccessError(-1002, "Can't reach the server. Make sure you have Internet connection and try again.");

		[JsonPropertyAttribute(PropertyName = "code")]
		public int InternalCode { get; private set; }

		[JsonPropertyAttribute(PropertyName = "message")]
		public string LocalizedMessage { get; private set; }

		public Exception @Exception { get; set; }

		public ServiceAccessError() : base()
		{
		}

		public ServiceAccessError(int internalCode, string localizedMessage) : base()
		{
			InternalCode = internalCode;
			LocalizedMessage = localizedMessage;
		}

		public override string ToString()
		{
			return string.Format("[Error] (\n InternalCode = {0},\n  LocalizedMessage = {1}\n)", InternalCode, LocalizedMessage);
		}
	}
}

