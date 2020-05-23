using System.Net;

namespace FatalForceServer.Engine.Models
{
    public class SocketReceivedResult
    {
        public int ReceivedBytes { get; set; }
        public EndPoint RemoteEndPoint { get; set; }
        public byte[] ReceivedData { get; set; }
    }
}
