using FatalForceServer.Core.Enumerations;
using System.Net;

namespace FatalForceServer.Core.Packets
{
    public abstract class Packet
    {
        public PacketInfo Header { get; set; }

        public Packet()
        {
            Header = new PacketInfo();
        }

        public void SetHeader(PacketTypeEnum eventType, EndPoint clientEndpoint)
        {
            Header.Sender = clientEndpoint;
            Header.SocketEventType = eventType;
        }

        public abstract byte[] Serialize();
        public abstract Packet Deserialize(byte[] buffer);
    }
}
