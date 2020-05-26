using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FatalForceServer.Core.Models
{
    public class WorldState
    {
        public List<PlayerState> PlayersStates { get; set; }

        public WorldState()
        {
            PlayersStates = new List<PlayerState>();
        }

        public byte[] Serialize()
        {
            var bytes = new List<byte>();

            bytes.AddRange(PlayersStates.SelectMany(playerState => playerState.Serialize()));

            return bytes.ToArray();
        }

        public WorldState Deserialize(byte[] buffer)
        {
            var playersStates = new List<PlayerState>();

            using (var stream = new MemoryStream(buffer))
            {
                using (var binaryReader = new BinaryReader(stream))
                {
                    int index = 0;

                    while (buffer.Length > index)
                    {
                        var playerState = new PlayerState();
                        playerState.X = binaryReader.ReadSingle();
                        playerState.Y = binaryReader.ReadSingle();
                        playerState.Id = binaryReader.ReadInt32();

                        playersStates.Add(playerState);

                        index = (int)binaryReader.BaseStream.Position;
                    }
                }
            }

            PlayersStates = playersStates;

            return this;
        }
    }
}
