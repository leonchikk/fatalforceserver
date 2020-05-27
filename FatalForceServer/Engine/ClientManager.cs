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

        //TODO Consider to refactor this
        public async Task<IEnumerable<int>> CheckClientsAvailableAsync(long allowedTimeout)
        {
            var notAvailableClients = new List<int>();
            var allAvailableClients = _connectionManager.GetAllAvailableRecipients();

            foreach (var client in allAvailableClients)
            {
                var clientTimeSpanFromLastPing = DateTime.UtcNow - (new DateTime(client.LastPingTimeStamp));

                if (clientTimeSpanFromLastPing.TotalMilliseconds >= allowedTimeout)
                {
                    _connectionManager.RemoveConnection(client.Id);
                    notAvailableClients.Add(client.Id);

                    await DisconnectAsync(client, "Connection timeout");
                }
            }

            return notAvailableClients;
        }

        public async Task DisconnectAsync(ClientConnection client, string reason)
        {
            var disconnectPacket = new DisconnectPacket(client.Id, reason);

            await _socketManager.SendAsync(
                    data: disconnectPacket.Serialize(),
                    recipients: _connectionManager.GetAllAvailableRecipients()
                 );
        }

        public async Task PingClientsAsync()
        {
            var pingPacket = new PingPacket();
            await _socketManager.SendAsync(
                            data: pingPacket.Serialize(),
                            recipients: _connectionManager.GetAllAvailableRecipients()
                        );
        }
    }
}
