using System;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public static class HttpConfig
    {
		public static string Protocol = "http://";
//        public static string Host = "staging.bettersalesman.com";
        public static string Host = "sap.t.proxylocal.com";
		public static string ApiBaseAddress = string.Format("{0}{1}/api/v1/", Protocol, Host);

        static string lang;

        public static string Lang
        {
            get
            {
                if (string.IsNullOrEmpty(lang))
                {
                    throw new NotImplementedException("Lang value must be filled with current locale information before read");
                }

                return lang;
            }

            set { lang = value; }
        }
    }
}

