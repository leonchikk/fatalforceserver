using System;
using System.Collections.Generic;

namespace FatalForceServer.Core.Models
{
    public class PlayerState
    {
        public float Speed { get; set; } = 3f;
        public float X { get; set; }
        public float Y { get; set; }
        public int Id { get; set; }

        public PlayerState() { }
        public PlayerState(int id)
        {
            Id = id;
        }

        public byte[] Serialize()
        {
            var bytes = new List<byte>();

            bytes.AddRange(BitConverter.GetBytes(X));
            bytes.AddRange(BitConverter.GetBytes(Y));
            bytes.AddRange(BitConverter.GetBytes(Id));

            return bytes.ToArray();
        }

        public void InitPosition()
        {
            var randomizer = new Random();

            X = randomizer.Next(-5, 5);
            Y = randomizer.Next(-5, 5);
        }
    }
}
