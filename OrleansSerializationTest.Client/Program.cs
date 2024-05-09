using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans.Configuration;
using Orleans.Serialization;
using OrleansSerializationTest.Models;
using OrleansSerializationTest.Models.GrainInterfaces;

IHostBuilder builder = Host.CreateDefaultBuilder(args)
    .UseOrleansClient(client =>
    {
        client.UseConsulClientClustering(consulOptions =>
        {
            consulOptions.ConfigureConsulClient(new Uri("http://localhost:8500"));
        });

        client.Configure<ClusterOptions>(options =>
        {
            options.ClusterId = "streams-test";
            options.ServiceId = "StreamsTest";
        });

        client.Services.AddSerializer(serializerBuilder => serializerBuilder.AddJsonSerializer(isSupported: (type) =>
        {
            return type.Name.Contains("TestModelWithoutSerializer");
        }));

    })
    .ConfigureLogging(logging => logging.AddConsole())
    .UseConsoleLifetime();

using IHost host = builder.Build();
await host.StartAsync();

IClusterClient client = host.Services.GetRequiredService<IClusterClient>();

await Task.Delay(20000);
Console.WriteLine("Starting test");

for (int i = 0; i < StreamSettings.MEMORY_STREAM_CONSUMER_COUNT; i++)
{
    var id = StreamIds.GetGrainIdForStream();
    await client.GetGrain<IConsumerGrainMemoryStream>(id).Init();
}

Console.ReadKey();

await host.StopAsync();