using FatalForceServer.Core;
using FatalForceServer.Core.Packets;
using FatalForceServer.Engine.Interfaces;
using FatalForceServer.Engine.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;

namespace FatalForceServer.Engine
{
    public class ConnectionManager: IConnectionManager
    {
        private readonly ISocketManager _socketManager;
        private readonly ConcurrentDictionary<int, Client> _connections;
        private int _clientIdentifierCounter;

        public ConnectionManager(ISocketManager socketManager)
        {
            _socketManager = socketManager;
            _connections = new ConcurrentDictionary<int, Client>();
            _clientIdentifierCounter = 0;
        }

        public int AddConnection(ConnectionPacket connectionPacket)
        {
            _clientIdentifierCounter += 1;

            _connections.TryAdd(_clientIdentifierCounter, new Client()
            {
                Id = _clientIdentifierCounter,
                EndPoint = connectionPacket.Header.Sender,
                Nickname = connectionPacket.Nickname,
                LastPingTimeStamp = DateTime.UtcNow.Ticks
            });

            Log.Info($"{connectionPacket.Nickname} has been connected");

            return _clientIdentifierCounter;
        }

        public Client[] GetAllAvailableRecipients()
        {
            return _connections.Values.ToArray();
        }

        public void RemoveConnection(int clientId)
        {
            _connections.TryRemove(clientId, out Client client);

            Log.Info($"{client.Nickname} has been disconnected");
        }

        public void SetAsPinged(int clientId)
        {
            if (!_connections.ContainsKey(clientId))
                return;

            var client = _connections[clientId];

            client.LastPingTimeStamp = DateTime.UtcNow.Ticks;
        }

        public Client GetClientById(int clidentId)
        {
            return _connections[clidentId];
        }
    }
}
