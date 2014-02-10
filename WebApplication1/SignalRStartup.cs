
using System;
using System.Configuration;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;
using WebApplication1;

[assembly:OwinStartup(typeof(SignalRStartup))]
namespace WebApplication1
{
    public class FooHub : Hub
    {
        public string Hello()
        {
            return "hi";
        }
    }

    public class SignalRStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var hubConfig = new HubConfiguration
            {
#if DEBUG
                EnableDetailedErrors = true
#else
                    EnableDetailedErrors = false
#endif
            };


#if DEBUG
            const bool defaultVal = true;
#else
    // Low-level logging off by default in release mode, can be overridden by app setting
            const bool defaultVal = false;
#endif
            // Trace SignalR at a low level
            //if (new ConfigProvider().GetBoolAppSetting("SignalRPipelineLogging", defaultVal))
            //{
            //    LoggingPipelineModule.EnableLogging();
            //}

            GlobalHost.DependencyResolver.UseSqlServer(
                new SqlScaleoutConfiguration(
                    ConfigurationManager.ConnectionStrings["CatalystMessaging"].ConnectionString));

            GlobalHost.Configuration.TransportConnectTimeout = TimeSpan.FromMinutes(5);

            // Branch the pipeline here for requests that start with "/signalr"
            app.Map("/signalr", map =>
            {
                // Setup the cors middleware to run before SignalR.
                // By default this will allow all origins. You can 
                // configure the set of origins and/or http verbs by
                // providing a cors options with a different policy.
                map.UseCors(CorsOptions.AllowAll);
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch is already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfig);
            });
        }
    }
}