using FatalForceServer.Core.Packets;
using FatalForceServer.Engine.Models;

namespace FatalForceServer.Engine.Interfaces
{
    public interface IConnectionManager
    {
        int AddConnection(ConnectionPacket connectionPacket);
        Client[] GetAllAvailableRecipients();
        Client GetClientById(int clidentId);
        void Disconnect(int clientId);
        void SetAsPinged(int clientId);
    }
}
