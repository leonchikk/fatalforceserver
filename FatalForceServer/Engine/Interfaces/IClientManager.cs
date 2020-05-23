using FatalForceServer.Engine.Models;
using System.Threading.Tasks;

namespace FatalForceServer.Engine.Interfaces
{
    public interface IClientManager
    {
        Task DisconnectAsync(Client client);
        Task KickClientAsync(Client client, string reason);
        Task PingClientsAsync();
        Task CheckClientsAvailableAsync(long allowedTimeout);
    }
}
