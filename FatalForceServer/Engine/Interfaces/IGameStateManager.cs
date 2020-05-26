using FatalForceServer.Core.Core.Enumerations;
using FatalForceServer.Core.Core.Models;
using System.Numerics;
using System.Threading.Tasks;

namespace FatalForceServer.Engine.Interfaces
{
    public interface IGameStateManager
    {
        void AddPlayer(int clientId);
        void RemovePlayer(int clientId);
        void MovePlayer(int clientId, PlayerMovementTypeEnum direction);
        WorldState GetLastWorldState();
        Task SendWorldStateToClients();
    }
}
