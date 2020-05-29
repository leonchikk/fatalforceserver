using FatalForceServer.Core.Packets;
using FatalForceServer.Engine.Interfaces;
using FatalForceServer.Engine.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FatalForceServer.Engine
{
    public class ClientManager : IClientManager
    {
        private readonly ISocketManager _socketManager;
        private readonly IConnectionManager _connectionManager;

        public ClientManager(ISocketManager socketManager, IConnectionManager connectionManager)
        {
            _socketManager = socketManager;
            _connectionManager = connectionManager;
        }

        public IEnumerable<ClientConnection> GetNotAvailableClients(long allowedTimeout)
        {
            var notAvailableClients = new List<ClientConnection>();
            var allAvailableClients = _connectionManager.GetAllAvailableRecipients();

            foreach (var client in allAvailableClients)
            {
                var clientTimeSpanFromLastPing = DateTime.UtcNow - (new DateTime(client.LastPingTimeStamp));

                if (clientTimeSpanFromLastPing.TotalMilliseconds >= allowedTimeout)
                {
                    notAvailableClients.Add(client);
                }
            }

            return notAvailableClients;
        }

        public async Task DisconnectAsync(IEnumerable<ClientConnection> clients, string reason)
        {
            foreach (var client in clients)
            {
                await DisconnectAsync(client, reason);
            }
        }

        public async Task DisconnectAsync(ClientConnection client, string reason)
        {
            _connectionManager.RemoveConnection(client.Id);

            var disconnectPacket = new DisconnectPacket(client.Id, reason);

            await _socketManager.SendAsync(
                    data: disconnectPacket.Serialize(),
                    recipients: _connectionManager.GetAllAvailableRecipients()
                 );
        }

        public async Task PingClientAsync(int clientId)
        {
            var recipient = _connectionManager.GetClientById(clientId);

            var pingPacket = new PingPacket();
            await _socketManager.SendAsync(
                            data: pingPacket.Serialize(),
                            recipient: recipient
                        );
        }
    }
}
