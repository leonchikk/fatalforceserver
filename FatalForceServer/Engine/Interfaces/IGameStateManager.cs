using FatalForceServer.Core.Enumerations;
using FatalForceServer.Core.Models;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace FatalForceServer.Engine.Interfaces
{
    public interface IGameStateManager
    {
        void AddPlayer(int clientId);
        void RemovePlayer(int clientId);
        void RemovePlayers(IEnumerable<int> clientsIds);
        void MovePlayer(int clientId, int packetSequenceNumber, PlayerMovementDirectionEnum direction);
        WorldState GetLastWorldState();
        Task SendWorldStateToClients();
    }
}
