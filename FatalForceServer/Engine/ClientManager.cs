using FatalForceServer.Core.Packets;
using FatalForceServer.Engine.Interfaces;
using FatalForceServer.Engine.Models;
using System;
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
        public async Task CheckClientsAvailableAsync(long allowedTimeout)
        {
            var recipients = _connectionManager.GetAllAvailableRecipients();

            foreach (var recipient in recipients)
            {
                var clientTimeSpanFromLastPing = DateTime.UtcNow - (new DateTime(recipient.LastPingTimeStamp));

                if (clientTimeSpanFromLastPing.TotalMilliseconds >= allowedTimeout)
                {
                    _connectionManager.RemoveConnection(recipient.Id);

                    var disconnectPacket = new DisconnectPacket(recipient.Id, "Connection timeout");

                    await _socketManager.SendAsync(
                            data: disconnectPacket.Serialize(),
                            recipients: _connectionManager.GetAllAvailableRecipients()
                         );
                }
            }
        }

        public async Task DisconnectAsync(Client client)
        {
            throw new NotImplementedException();
        }

        public async Task KickClientAsync(Client client, string reason)
        {
            throw new NotImplementedException();
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
