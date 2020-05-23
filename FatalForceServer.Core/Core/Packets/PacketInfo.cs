using System.Net;

namespace FatalForceServer.Core.Packets
{
    public class PacketInfo
    {
        public EndPoint Sender { get; set; }
        public PacketTypeEnum SocketEventType { get; set; }
    }
}
