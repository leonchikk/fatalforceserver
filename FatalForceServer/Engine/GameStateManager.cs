using FatalForceServer.Core.Enumerations;
using FatalForceServer.Core.Models;
using FatalForceServer.Core.Packets;
using FatalForceServer.Engine.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FatalForceServer.Engine
{
    public class GameStateManager : IGameStateManager
    {
        private readonly ISocketManager _socketManager;
        private readonly IConnectionManager _connectionManager;

        private readonly WorldState _lastWorldState;

        public GameStateManager(ISocketManager socketManager, IConnectionManager connectionManager)
        {
            _socketManager = socketManager;
            _connectionManager = connectionManager;

            _lastWorldState = new WorldState();
        }

        public void AddPlayer(int clientId)
        {
            var playerState = new PlayerState(clientId);
            playerState.InitPosition();

            _lastWorldState.PlayersStates.Add(playerState);
        }

        public void MovePlayer(int clientId, PlayerMovementDirectionEnum direction)
        {
            var playerState = _lastWorldState.PlayersStates.FirstOrDefault(p => p.Id == clientId);

            if (playerState == null)
                return;

            switch (direction)
            {
                case PlayerMovementDirectionEnum.Up:

                    playerState.Y += playerState.Speed;
                    break;

                case PlayerMovementDirectionEnum.Down:

                    playerState.Y += -playerState.Speed;
                    break;

                case PlayerMovementDirectionEnum.Right:

                    playerState.X += playerState.Speed;
                    break;

                case PlayerMovementDirectionEnum.Left:

                    playerState.X += -playerState.Speed;
                    break;

                default:
                    break;
            }
        }

        public WorldState GetLastWorldState()
        {
            return _lastWorldState;
        }

        public void RemovePlayer(int clientId)
        {
            var playerState = _lastWorldState.PlayersStates.FirstOrDefault(p => p.Id == clientId);

            if (playerState == null)
                return;

            _lastWorldState.PlayersStates.Remove(playerState);
        }

        public async Task SendWorldStateToClients()
        {
            var worldStatePacket = new WorldStatePacket();

            await _socketManager.SendAsync(
                data:        worldStatePacket.SetWorldState(_lastWorldState).Serialize(),
                recipients: _connectionManager.GetAllAvailableRecipients());
        }

        public void RemovePlayers(IEnumerable<int> clientsIds)
        {
            foreach (var clientId in clientsIds)
            {
                RemovePlayer(clientId);
            }
        }
    }
}
