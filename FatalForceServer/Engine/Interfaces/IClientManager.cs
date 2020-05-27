using FatalForceServer.Engine.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FatalForceServer.Engine.Interfaces
{
    public interface IClientManager
    {
        Task DisconnectAsync(IEnumerable<ClientConnection> clients, string reason);
        Task DisconnectAsync(ClientConnection client, string reason);
        Task PingClientsAsync();
        IEnumerable<ClientConnection> GetNotAvailableClients(long allowedTimeout);
    }
}
