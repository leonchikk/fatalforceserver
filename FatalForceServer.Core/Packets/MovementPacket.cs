using FatalForceServer.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.IO;

namespace FatalForceServer.Core.Packets
{
    public class MovementPacket : Packet
    {
        public int ClientId { get; set; }
        public PlayerMovementDirectionEnum Direction { get; set; }

        public override Packet Deserialize(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer, 1, buffer.Length - 1))
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    ClientId = binaryReader.ReadInt32();
                    Direction = (PlayerMovementDirectionEnum) binaryReader.ReadByte();

                    return this;
                }
            }
        }

        public override byte[] Serialize()
        {
            var data = new List<byte>();

            data.Add((byte)PacketTypeEnum.Movement);
            data.AddRange(BitConverter.GetBytes(ClientId));
            data.Add((byte)Direction);

            return data.ToArray();
        }
    }
}
