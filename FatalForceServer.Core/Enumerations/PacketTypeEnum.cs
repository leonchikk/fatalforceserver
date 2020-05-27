namespace FatalForceServer.Core.Enumerations
{
    public enum PacketTypeEnum
    {
        Connection = 1,
        AcceptConnection = 2,
        Disconnect = 3,
        Ping = 4,
        WorldState = 5,
        Movement = 6
    }
}
