using Orleans.Runtime;
using Orleans.Streams;
using OrleansSerializationTest.Models;
using OrleansSerializationTest.Models.GrainInterfaces;

namespace OrleansSerializationTest.Silo
{
    public class ConsumerGrainMemoryStream : Grain, IConsumerGrainMemoryStream
    {
        private IAsyncStream<StreamMessage>? _stream;

        private readonly GrainIdData _id;
        private readonly PortInfo _portInfo;

        public ConsumerGrainMemoryStream(PortInfo portInfo) : base()
        {
            _id = StreamIds.GetGrainKeyData(this.GetPrimaryKeyString());
            _portInfo = portInfo;
        }

        public override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            var streamProvider = this.GetStreamProvider($"{StreamSettings.MEMORY_STREAM_PROVIDER}_{_portInfo.SiloPort}");
            var streamId = StreamId.Create($"{StreamSettings.MEMORY_STREAM_NAMESPACE}", _id.StreamId);
            _stream = streamProvider.GetStream<StreamMessage>(streamId);

            await _stream.SubscribeAsync(
                async (data, token) =>
                {
                    if (_portInfo.SiloPort != data.SiloPort)
                    {
                        Console.WriteLine($"Silo port/messsage port {_portInfo.SiloPort}/{data.SiloPort}");
                    }
                    await Task.CompletedTask;
                });


            await base.OnActivateAsync(cancellationToken);
        }

        public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Deactivating ConsumerGrainMemoryStream {this.GetPrimaryKeyString()} {reason.ReasonCode} {reason.Description} {reason.Exception}");

            return base.OnDeactivateAsync(reason, cancellationToken);
        }

        public async Task Init()
        {
            await GrainFactory.GetGrain<IPublisherGrainMemoryStream>($"{_id.StreamId}_{_portInfo.SiloPort}").Init();
        }
    }
}
