using System.Threading.Tasks;
using FatalForceServer.Core.Packets;
using FatalForceServer.Engine.Interfaces;

namespace FatalForceServer.Logic
{
    public class GameProcessManager : IGameProcessManager
    {
        private readonly IClientManager _clientManager;
        private readonly IConnectionManager _connectionManager;
        private readonly IGameStateManager _gameStateManager;
        private readonly ISocketManager _socketManager;

        public GameProcessManager(
            IClientManager clientManager,
            IConnectionManager connectionManager,
            IGameStateManager gameStateManager,
            ISocketManager socketManager)
        {
            _clientManager = clientManager;
            _connectionManager = connectionManager;
            _gameStateManager = gameStateManager;
            _socketManager = socketManager;
        }

        public async Task ProcessIncomingPacketAsync(Packet packet)
        {
            if (packet is ConnectionPacket)
            {
                var connectionPacket = packet as ConnectionPacket;
                var acceptPacket = new AcceptConnectionPacket();

                var addedClient = _connectionManager.AddConnection(connectionPacket);
                var connectedClient = _connectionManager.GetClientById(addedClient.Id);

                _gameStateManager.AddPlayer(connectedClient.Id);

                var currentWorldState = _gameStateManager.GetLastWorldState();

                await _socketManager.SendAsync(acceptPacket.SetIdentifier(addedClient.Id)
                                                           .SetWorldState(currentWorldState)
                                                           .Serialize(),
                                               connectedClient);

                await _socketManager.SendAsync(connectionPacket
                                                            .SetIdentifier(addedClient.Id)
                                                            .Serialize(),
                    recipients: _connectionManager.GetAllAvailableRecipients(),
                    except: connectedClient
                 );
            }

            if (packet is PingPacket)
            {
                var pingPacket = packet as PingPacket;

                _connectionManager.SetAsPinged(pingPacket.ClientId);

                await _clientManager.PingClientAsync(pingPacket.ClientId);
            }

            if (packet is MovementPacket)
            {
                var movementPacket = packet as MovementPacket;

                _gameStateManager.MovePlayer(movementPacket.ClientId, movementPacket.Direction);
            }
        }
    }
}
