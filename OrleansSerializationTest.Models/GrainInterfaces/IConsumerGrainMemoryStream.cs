using Orleans.Concurrency;

namespace OrleansSerializationTest.Models.GrainInterfaces
{
    public interface IConsumerGrainMemoryStream : IGrainWithStringKey
    {
        [AlwaysInterleave]
        Task Init();
    }
}
