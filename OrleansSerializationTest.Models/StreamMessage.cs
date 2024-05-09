namespace OrleansSerializationTest.Models
{
    [GenerateSerializer]
    public class StreamMessage
    {
        [Id(0)]
        public Guid Id { get; set; }
        [Id(1)]
        public DateTime Timestamp { get; set; }
        [Id(3)]
        public int SiloPort { get; set; }
    }
}
