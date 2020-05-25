using FatalForceServer.Core.Enumerations;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;

namespace FatalForceServer.Core.Packets
{
    public class ConnectionPacket: Packet
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public float XPosition { get; set; }
        public float YPosition { get; set; }

        public ConnectionPacket SetIdentifier(int id)
        {
            Id = id;
            return this;
        }

        public ConnectionPacket SetNickname(string nickname)
        {
            Nickname = nickname;
            return this;
        }

        public ConnectionPacket SetPosition(Vector2 position)
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

                    var nickNameStringLength = binaryReader.ReadInt32();
                    var nickNameBytes = binaryReader.ReadBytes(nickNameStringLength);

                    XPosition = binaryReader.ReadSingle();
                    YPosition = binaryReader.ReadSingle();

                    Nickname = Encoding.UTF8.GetString(nickNameBytes);

                    return this;
                }
            }
        }

        public override byte[] Serialize()
        {
            var data = new List<byte>();

            data.Add((byte)PacketTypeEnum.Connection);
            data.AddRange(BitConverter.GetBytes(Id));
            data.AddRange(BitConverter.GetBytes(Nickname.Length));
            data.AddRange(Encoding.UTF8.GetBytes(Nickname));
            data.AddRange(BitConverter.GetBytes(XPosition));
            data.AddRange(BitConverter.GetBytes(YPosition));

            return data.ToArray();
        }
    }
}
