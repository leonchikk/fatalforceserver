using FatalForceServer.Engine.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FatalForceServer.Engine.Interfaces
{
    public interface IClientManager
    {
        Task DisconnectAsync(ClientConnection client, string reason);
        Task PingClientsAsync();
        Task<IEnumerable<int>> CheckClientsAvailableAsync(long allowedTimeout);
    }
}
