using Orleans.Runtime;
using Orleans.Runtime.Placement;

namespace OrleansSerializationTest.Silo
{
    public class ForcedLocalPlacementDirector : IPlacementDirector
    {
        private readonly string[] OVERRIDE_TO_LOCAL = new string[] { "pubsubrendezvous", "memorystreamqueue" };

        private static Random _rng = new Random();
        public Task<SiloAddress> OnAddActivation(
            PlacementStrategy strategy,
            PlacementTarget target,
            IPlacementContext context)
        {
            var silos = context.GetCompatibleSilos(target).OrderBy(s => s).ToArray();
            Console.WriteLine($"Activating {target.GrainIdentity.Type.ToString()} grain from silo {context.LocalSilo}");

            if (OVERRIDE_TO_LOCAL.Contains(target.GrainIdentity.Type.ToString()))
            {
                return Task.FromResult(context.LocalSilo);
            }

            return Task.FromResult(silos[_rng.Next(0, silos.Length)]);
        }
    }

}
