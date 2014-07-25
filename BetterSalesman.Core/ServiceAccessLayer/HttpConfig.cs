using System;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public static class HttpConfig
    {
        public static string Host = "staging.bettersalesman.com";
        public static string Protocol = "http://";

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

