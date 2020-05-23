using FatalForceServer.Core;
using FatalForceServer.Core.Packets;
using FatalForceServer.Engine.Models;
using System.IO;

namespace FatalForceServer.Engine.Extensions
{
    public static class BytesExtension
    {
        public static PacketTypeEnum GetSocketEventType(this byte[] receivedBuffer)
        {
            using (var stream = new MemoryStream(receivedBuffer))
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    return (PacketTypeEnum)binaryReader.ReadByte();
                }
            }
        }

        public static Packet ConvertToPacket(this byte[] buffer, SocketReceivedResult socketReceiveResult)
        {
            Packet receivedPacket = null;
            var eventType = buffer.GetSocketEventType();

            switch (eventType)
            {
                case PacketTypeEnum.Connection:
                    receivedPacket = new ConnectionPacket();
                    break;

                case PacketTypeEnum.Ping:
                    receivedPacket = new PingPacket();
                    break;

                case PacketTypeEnum.Disconnect:
                    receivedPacket = new DisconnectPacket();
                    break;

                default:
                    Log.Error("Unknown packet type, cannot handle them");
                    break;
            }

            receivedPacket.Deserialize(buffer);
            receivedPacket.SetHeader(eventType, socketReceiveResult.RemoteEndPoint);

            return receivedPacket;
        }
    }
}
