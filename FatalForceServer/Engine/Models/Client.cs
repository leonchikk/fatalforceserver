using System;
using System.Net;
using System.Numerics;

namespace FatalForceServer.Engine.Models
{
    public class Client
    {
        public int Id { get; set; }
        public EndPoint EndPoint { get; set; }
        public string Nickname { get; set; }
        public long LastPingTimeStamp { get; set; }
        public Vector2 Position { get; set; }
        public float Speed { get; set; }

        public void InitPosition()
        {
            var randomizer = new Random();

            var x = randomizer.Next(-5, 5);
            var y = randomizer.Next(-5, 5);

            Position = new Vector2(x, y);
        }
    }
}
