namespace OrleansSerializationTest.Models
{
    public class GrainIdData
    {
        public Guid StreamId { get; set; }
        public Guid GrainId { get; set; }
    }

    public static class StreamIds
    {
        private static readonly Random _rng = new Random();

        private static readonly Guid[] _availableStreams = new Guid[]
        {
            Guid.Parse("550e8400-e29b-41d4-a716-446655440000"),
            Guid.Parse("550e8400-e29b-41d4-a716-446655440001"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440002"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440003"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440004"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440005"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440006"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440007"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440008"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440009"),

            //Guid.Parse("550e8400-e29b-41d4-a716-446655440010"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440011"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440012"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440013"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440014"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440015"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440016"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440017"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440018"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440019"),

            //Guid.Parse("550e8400-e29b-41d4-a716-446655440020"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440021"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440022"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440023"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440024"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440025"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440026"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440027"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440028"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440029"),

            //Guid.Parse("550e8400-e29b-41d4-a716-446655440030"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440031"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440032"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440033"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440034"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440035"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440036"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440037"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440038"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440039"),

            //Guid.Parse("550e8400-e29b-41d4-a716-446655440040"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440041"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440042"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440043"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440044"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440045"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440046"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440047"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440048"),
            //Guid.Parse("550e8400-e29b-41d4-a716-446655440049"),
        };


        public static Guid GetStreamId()
        {
            return _availableStreams[_rng.Next(0, _availableStreams.Length)];
        }

        public static string GetGrainIdForStream()
        {
            return $"{GetStreamId()}|{Guid.NewGuid()}";
        }

        public static string GetGrainIdForStream(Guid streamId)
        {
            return $"{streamId}|{Guid.NewGuid()}";
        }

        public static GrainIdData GetGrainKeyData(string grainKey)
        {
            var splitted = grainKey.Split('|');
            return new GrainIdData
            {
                StreamId = Guid.Parse(splitted[0]),
                GrainId = Guid.Parse(splitted[1]),
            };
        }

        public static Guid GetLocalStreamId(Guid streamId, int siloPort)
        {
            // Convert the integer to a byte array
            byte[] intValueBytes = BitConverter.GetBytes(siloPort);

            // Ensure the byte array is 4 bytes long, padding with zeros if necessary
            while (intValueBytes.Length < 4)
            {
                intValueBytes = new byte[intValueBytes.Length + 1];
                Array.Copy(intValueBytes, 0, intValueBytes, 1, intValueBytes.Length - 1);
                intValueBytes[0] = 0;
            }

            // Replace the last 4 bytes of the existing Guid with the integer bytes
            byte[] combinedBytes = streamId.ToByteArray();
            Array.Copy(intValueBytes, 0, combinedBytes, combinedBytes.Length - 4, 4);

            // Return a new Guid constructed from the combined bytes
            return new Guid(combinedBytes);
        }
    }
}
