using FatalForceServer.Core.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FatalForceServer.Core.Packets
{
    public class DisconnectPacket: Packet
    {
        public int ClientId { get; set; }
        public string Reason { get; set; }

        public DisconnectPacket() { }
        public DisconnectPacket(int clientId, string reason)
        {
            ClientId = clientId;
            Reason = reason;
        }

        public override Packet Deserialize(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer, 1, buffer.Length - 1))
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    var reasonStringLength = binaryReader.ReadInt32();
                    var nickNameBytes = binaryReader.ReadBytes(reasonStringLength);

                    return this;
                }
            }
        }

        public override byte[] Serialize()
        {
            var data = new List<byte>();

            data.Add((byte)PacketTypeEnum.Disconnect);
            data.AddRange(BitConverter.GetBytes(ClientId));
            data.AddRange(BitConverter.GetBytes(Reason.Length));
            data.AddRange(Encoding.UTF8.GetBytes(Reason));

            return data.ToArray();
        }
    }
}
