﻿using FatalForceServer.Core.Models;
using FatalForceServer.Core.Enumerations;
using System.Collections.Generic;
using System.IO;
using System;

namespace FatalForceServer.Core.Packets
{
    public class WorldStatePacket : Packet
    {
        public WorldState WorldState { get; set; }

        public WorldStatePacket SetWorldState(WorldState worldState)
        {
            WorldState = worldState;
            return this;
        }

        public override Packet Deserialize(byte[] buffer)
        {
            var worldState = new WorldState();

            using (var stream = new MemoryStream(buffer, 1, buffer.Length - 1))
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    var worldStateBytesCount = binaryReader.ReadInt32();
                    var worldStateBytes = binaryReader.ReadBytes(worldStateBytesCount);

                    WorldState = worldState.Deserialize(worldStateBytes);

                    return this;
                }
            }
        }

        public override byte[] Serialize()
        {
            var bytes = new List<byte>();
            var worldStateBytes = WorldState.Serialize();

            bytes.Add((byte)PacketTypeEnum.WorldState);
            bytes.AddRange(BitConverter.GetBytes(worldStateBytes.Length));
            bytes.AddRange(worldStateBytes);

            return bytes.ToArray();
        }
    }
}
