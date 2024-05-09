using Orleans.Placement;
using Orleans.Runtime;
using Orleans.Streams;
using OrleansSerializationTest.Models;
using OrleansSerializationTest.Models.GrainInterfaces;

namespace OrleansSerializationTest.Silo
{
    [PreferLocalPlacement]
    public class PublisherGrainMemoryStream : Grain, IPublisherGrainMemoryStream
    {
        private IDisposable? _publishTimer;
        private IDisposable? _deactivationTimer;
        private IAsyncStream<StreamMessage>? _stream;
        private readonly PortInfo _portInfo;

        private readonly GrainIdData _id;

        public PublisherGrainMemoryStream(PortInfo portInfo) : base()
        {
            var splited = this.GetPrimaryKeyString().Split('_');
            _id = new GrainIdData { StreamId = Guid.Parse(splited[0]), GrainId = Guid.Empty };
            _portInfo = portInfo;
        }

        public override async Task OnActivateAsync(CancellationToken cancellationToken)
        {
            _publishTimer = RegisterTimer(
                async (o) => await PublishMessage(),
                null,
                TimeSpan.FromMilliseconds(StreamSettings.MEMORY_STREAM_PUBLISH_INTERVAL),
                TimeSpan.FromMilliseconds(StreamSettings.MEMORY_STREAM_PUBLISH_INTERVAL)
            );

            _deactivationTimer = RegisterTimer(
                (o) =>
                {
                    _publishTimer.Dispose();
                    _deactivationTimer.Dispose();
                    this.DeactivateOnIdle();
                    return Task.CompletedTask;
                },
                null,
                TimeSpan.FromMilliseconds(StreamSettings.MEMORY_STREAM_PRODUCER_DEACTIVATION_INTERVAL),
                TimeSpan.FromDays(1)
            );

            var streamProvider = this.GetStreamProvider($"{StreamSettings.MEMORY_STREAM_PROVIDER}_{_portInfo.SiloPort}");
            var streamId = StreamId.Create($"{StreamSettings.MEMORY_STREAM_NAMESPACE}", _id.StreamId);
            _stream = streamProvider.GetStream<StreamMessage>(streamId);

            await base.OnActivateAsync(cancellationToken);
        }

        public override Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Deactivating PublisherGrainMemoryStream {this.GetPrimaryKeyString()} {reason.ReasonCode} {reason.Description} {reason.Exception}");

            return base.OnDeactivateAsync(reason, cancellationToken);
        }

        public Task Init()
        {
            return Task.CompletedTask;
        }

        private async Task PublishMessage()
        {
            Console.WriteLine($"Publishing message to {_portInfo.SiloPort}");
            await _stream.OnNextAsync(new StreamMessage { Id = Guid.NewGuid(), Timestamp = DateTime.Now, SiloPort = _portInfo.SiloPort });
        }
    }
}
