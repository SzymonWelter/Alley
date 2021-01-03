using System;

namespace Alley.Utils.Helpers
{
    public static class FormatHelper
    {
        public static string FormatServiceFullName(string package, string name) => 
            $"{package}.{name}";

        public static string FormatMicroserviceInstance(string serviceName, Uri uri) =>
            $"{{Microservice: {serviceName}, Uri: {uri}}}";
    }
}