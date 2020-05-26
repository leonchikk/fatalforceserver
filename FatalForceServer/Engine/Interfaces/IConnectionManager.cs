using FatalForceServer.Core.Packets;
using FatalForceServer.Engine.Models;

namespace FatalForceServer.Engine.Interfaces
{
    public interface IConnectionManager
    {
        ClientConnection AddConnection(ConnectionPacket connectionPacket);
        ClientConnection[] GetAllAvailableRecipients();
        ClientConnection GetClientById(int clidentId);
        void RemoveConnection(int clientId);
        void SetAsPinged(int clientId);
    }
}
