using System;
using MonoTouch.Foundation;
using System.Diagnostics;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public static class HttpConfig
    {
		public static string Protocol = "http://";
        public static string Host = "bettersalesman.com";
//        public static string Host = "sap.t.proxylocal.com";
		public static string ApiBaseAddress = string.Format("{0}{1}/api/v1/", Protocol, Host);

        public static string Lang
        {
            get
            {
                return Language();
            }
        }
        
        static string Language()
        {
            var currentLocale = NSLocale.PreferredLanguages[0];

            Debug.WriteLine("Current locale: " + currentLocale);

            return currentLocale;
        }
    }
}

