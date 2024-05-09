using Orleans.Concurrency;

namespace OrleansSerializationTest.Models.GrainInterfaces
{
    public interface IPublisherGrainMemoryStream : IGrainWithStringKey
    {
        [AlwaysInterleave]
        Task Init();
    }
}
