using System;
using Newtonsoft.Json;

namespace BetterSalesman.Core.ServiceAccessLayer
{
	public class ServiceAccessError
	{
		public static ServiceAccessError ErrorUnknown
		{ 
			get { return new ServiceAccessError(-1000, Localized("Something went wrong. Please try again later.")); } 
		}

		public static ServiceAccessError ErrorFileNotFound
		{
			get { return new ServiceAccessError(-1001, Localized("Couldn't find the requested file.")); }
		}

		public static ServiceAccessError ErrorHostUnreachable
		{
			get { return new ServiceAccessError(-1002, Localized("Can't reach the server. Make sure you have an active Internet connection and try again.")); }
		}

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

		private static string Localized(string localizationKey)
		{
			return Localization.Instance.GetLocalizedText(localizationKey);
		}
	}
}

