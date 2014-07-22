﻿using System;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public static class HttpConfig
    {
//        public static string Host = "www.json-generator.com"; // TODO stub backend
        public static string Host = "saapp-staging.herokuapp.com";
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

