namespace OrleansSerializationTest.Models
{
    public static class StreamSettings
    {
        public const string MEMORY_STREAM_PROVIDER = "MEMORY_STREAM_PROVIDER";
        public const int MEMORY_STREAM_PUBLISH_INTERVAL = 10;
        public const int MEMORY_STREAM_PARTITIONS_COUNT = 8;
        public const int MEMORY_STREAM_PRODUCER_DEACTIVATION_INTERVAL = 1000 * 60 * 60;
        public const string MEMORY_STREAM_NAMESPACE = "RANDOMDATA";

        public const int MEMORY_STREAM_CONSUMER_COUNT = 10;
    }
}
