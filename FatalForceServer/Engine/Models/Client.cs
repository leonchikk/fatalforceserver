using System.Net;

namespace FatalForceServer.Engine.Models
{
    public class Client
    {
        public int Id { get; set; }
        public EndPoint EndPoint { get; set; }
        public string Nickname { get; set; }
        public long LastPingTimeStamp { get; set; }
    }
}
