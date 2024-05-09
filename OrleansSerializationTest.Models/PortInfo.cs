namespace OrleansSerializationTest.Models
{
    public class PortInfo
    {
        public PortInfo()
        { }

        public PortInfo(int siloPort, int gatewayPort)
        {
            SiloPort = siloPort;
            GatewayPort = gatewayPort;
            RandomSiloPrefix = Guid.NewGuid().ToString();
        }
        public int SiloPort { get; set; }
        public int GatewayPort { get; set; }
        public string RandomSiloPrefix { get; set; }
    }
}
