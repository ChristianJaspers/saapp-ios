using System;
using Newtonsoft.Json;

namespace BetterSalesman.Core.ServiceAccessLayer
{
	public class ServiceAccessError
	{
		public static ServiceAccessError ErrorUnknown
		{ 
			get { return new ServiceAccessError(2101, Localized("something_went_wrong")); } 
		}

		public static ServiceAccessError ErrorHostUnreachable
		{
			get { return new ServiceAccessError(2102, Localized("cant_reach_server")); }
		}

		public static ServiceAccessError ErrorFileNotFound
		{
			get { return new ServiceAccessError(2201, Localized("file_not_found")); }
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

