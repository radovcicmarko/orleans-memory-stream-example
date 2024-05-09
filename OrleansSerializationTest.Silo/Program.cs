using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Runtime;
using OrleansSerializationTest.Models;
using OrleansSerializationTest.Silo;

var portInfo = new PortInfo(FreePorts.Find(), FreePorts.Find());

IHostBuilder builder = Host.CreateDefaultBuilder(args)
    .UseOrleans(silo =>
    {
        silo.Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "streams-test";
            options.ServiceId = "StreamsTest";
        });

        silo.ConfigureEndpoints(portInfo.SiloPort, portInfo.GatewayPort);
        silo.UseConsulSiloClustering(consulOptions =>
        {
            consulOptions.ConfigureConsulClient(new Uri("http://localhost:8500"));
        })
        .ConfigureLogging(logging => logging.AddConsole());
        silo.AddMemoryStreams($"{StreamSettings.MEMORY_STREAM_PROVIDER}_{portInfo.SiloPort}", options =>
        {
            options.ConfigureStreamPubSub(Orleans.Streams.StreamPubSubType.ExplicitGrainBasedOnly);
            options.ConfigurePartitioning(StreamSettings.MEMORY_STREAM_PARTITIONS_COUNT);
        }).AddMemoryGrainStorage("PubSubStore", options =>
        {
            options.NumStorageGrains = 10;
        });
        silo.UseDashboard((options) =>
        {
            options.Port = 1111;
            options.CounterUpdateIntervalMs = 10000;
        });
        silo.AddPlacementDirector<RandomPlacement, ForcedLocalPlacementDirector>();
    })
    .UseConsoleLifetime();

builder.ConfigureServices((services) =>
{
    services.AddSingleton(portInfo);
});

using IHost host = builder.Build();

await host.RunAsync();