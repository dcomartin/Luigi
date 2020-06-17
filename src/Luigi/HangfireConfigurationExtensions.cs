using System;
using Hangfire;
using Newtonsoft.Json;

namespace Luigi
{
    public static class HangfireConfigurationExtensions
    {
        public static void UseLuigi(this IGlobalConfiguration configuration, IServiceProvider serviceProvider)
        {
            configuration.UseActivator(new LuigiHangfireJobActivator(serviceProvider));
            var jsonSerializerSettings =  new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
            };
            configuration.UseSerializerSettings(jsonSerializerSettings);
        }
    }
}