using FatalForceServer.Engine.Models;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FatalForceServer.Engine.Interfaces
{
    public interface ISocketManager
    {
        void Configure(ServerConfig config);
        Task<SocketReceivedResult> ReceiveFromAsync();
        Task SendAsync(byte[] data, ClientConnection recipient);
        Task SendAsync(byte[] data, IEnumerable<ClientConnection> recipients);
        Task SendAsync(byte[] data, IEnumerable<ClientConnection> recipients, params ClientConnection[] except);
    }
}
