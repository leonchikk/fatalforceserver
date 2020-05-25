using FatalForceServer.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace FatalForceServer.Core.Packets
{
    public class AcceptConnectionPacket : Packet
    {
        public int Id { get; set; }
        public float XPosition { get; set; }
        public float YPosition { get; set; }

        public AcceptConnectionPacket SetIdentifier(int id)
        {
            Id = id;
            return this;
        }

        public AcceptConnectionPacket SetPosition(Vector2 position)
        {
            XPosition = position.X;
            YPosition = position.Y;

            return this;
        }

        public override Packet Deserialize(byte[] buffer)
        {
            using (var stream = new MemoryStream(buffer, 1, buffer.Length - 1))
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    Id = binaryReader.ReadInt32();
                    XPosition = binaryReader.ReadSingle();
                    YPosition = binaryReader.ReadSingle();

                    return this;
                }
            }
        }

        public override byte[] Serialize()
        {
            var data = new List<byte>();

            data.Add((byte)PacketTypeEnum.AcceptConnection);
            data.AddRange(BitConverter.GetBytes(Id));
            data.AddRange(BitConverter.GetBytes(XPosition));
            data.AddRange(BitConverter.GetBytes(YPosition));

            return data.ToArray();
        }
    }
}
