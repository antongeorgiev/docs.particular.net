﻿using System;
using System.Threading.Tasks;
using NServiceBus;
using NServiceBus.Configuration.AdvanceExtensibility;
using NServiceBus.Transports.Http;
using NServiceBus.Wormhole.Gateway;

class Program
{
    static void Main()
    {
        AsyncMain().GetAwaiter().GetResult();
    }

    static async Task AsyncMain()
    {
        Console.Title = "Samples.Wormhole.PingPong.GatewayA";

        var gatewayConfig = new WormholeGatewayConfiguration<MsmqTransport, HttpTransport>("Gateway.SiteB", "SiteB");

        #region ConfigureGatewayB

        gatewayConfig.ConfigureRemoteSite("SiteA", "Gateway.SiteA");
        gatewayConfig.ForwardToEndpoint("Contracts", "Samples.Wormhole.PingPong.Server");

        #endregion

        //Hack necessary to make 6.3.x MSMQ work
        gatewayConfig.CustomizeLocalTransport((c, t) => t.GetSettings().Set("errorQueue", "poison"));

        var gateway = await gatewayConfig.Start()
            .ConfigureAwait(false);

        Console.WriteLine("Press <enter> to exit");
        Console.ReadLine();

        await gateway.Stop()
            .ConfigureAwait(false);
    }
}