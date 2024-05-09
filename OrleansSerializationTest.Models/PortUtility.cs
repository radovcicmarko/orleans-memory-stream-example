using System.Net;
using System.Net.Sockets;

namespace OrleansSerializationTest.Models
{
    public static class FreePorts
    {
        public static int Find()
        {
            int port = 0;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                IPEndPoint localEP = new IPEndPoint(IPAddress.Any, 0);
                socket.Bind(localEP);
                localEP = (IPEndPoint)socket.LocalEndPoint;
                port = localEP.Port;
            }
            finally
            {
                socket.Close();
            }
            return port;
        }
    }
}
