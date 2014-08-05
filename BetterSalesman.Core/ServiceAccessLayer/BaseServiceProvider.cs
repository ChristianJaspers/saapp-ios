using System.Collections.Generic;
using Newtonsoft.Json;

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
            [JsonPropertyAttribute(PropertyName = "notification_token")]
            public string NotificationToken;
            
            [JsonPropertyAttribute(PropertyName = "platform")]
            public string Platform;
            
            [JsonPropertyAttribute(PropertyName = "locale")]
            public string Locale;
        }
    }
}

