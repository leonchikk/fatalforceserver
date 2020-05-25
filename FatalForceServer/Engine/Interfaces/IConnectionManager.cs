using FatalForceServer.Core.Packets;
using FatalForceServer.Engine.Models;

namespace FatalForceServer.Engine.Interfaces
{
    public interface IConnectionManager
    {
        Client AddConnection(ConnectionPacket connectionPacket);
        Client[] GetAllAvailableRecipients();
        Client GetClientById(int clidentId);
        void RemoveConnection(int clientId);
        void SetAsPinged(int clientId);
    }
}
