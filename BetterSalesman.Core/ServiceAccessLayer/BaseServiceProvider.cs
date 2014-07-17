using System.Collections.Generic;

namespace BetterSalesman.Core.ServiceAccessLayer
{
    public class BaseServiceProvider
    {
        const string paramDeviceInfo = "device_info";
        const string paramCurrentPlatform = "ios";
        
        internal Dictionary<string, object> ParametersWithDeviceInfo(Dictionary<string, object> parameters)
        {
            parameters.Add(paramDeviceInfo, new Device {
                NotificationToken = "notification token goes here", // TODO
                Platform = paramCurrentPlatform,
                Locale = HttpConfig.Lang
            });

            return parameters;
        }

        class Device
        {
            public string NotificationToken;
            public string Platform;
            public string Locale;
        }
    }
}

