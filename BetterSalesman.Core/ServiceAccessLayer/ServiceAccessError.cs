using System;
using Newtonsoft.Json;

namespace BetterSalesman.Core
{
	public class ServiceAccessError
	{
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

