using System;
using System.Collections.Generic;
using System.IO;

namespace FatalForceServer.Core.Packets
{
    public class AcceptConnectionPacket : Packet
    {
        public int Id { get; set; }

        public Packet SetIdentifier(int id)
        {
            Id = id;
            return this;
        }

        public override Packet Deserialize(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer, 1, buffer.Length - 1))
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    Id = binaryReader.ReadInt32();

                    return this;
                }
            }
        }

        public override byte[] Serialize()
        {
            var data = new List<byte>();

            data.Add((byte)PacketTypeEnum.AcceptConnection);
            data.AddRange(BitConverter.GetBytes(Id));

            return data.ToArray();
        }
    }
}
