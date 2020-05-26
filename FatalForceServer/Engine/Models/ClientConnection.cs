using System;
using System.Net;
using System.Numerics;

namespace FatalForceServer.Engine.Models
{
    public class ClientConnection
    {
        public int Id { get; set; }
        public EndPoint EndPoint { get; set; }
        public string Nickname { get; set; }
        public long LastPingTimeStamp { get; set; }
    }
}
