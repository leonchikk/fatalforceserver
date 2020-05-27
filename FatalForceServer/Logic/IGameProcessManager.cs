using FatalForceServer.Core.Packets;
using System.Threading.Tasks;

namespace FatalForceServer.Logic
{
    public interface IGameProcessManager
    {
        Task ProcessIncomingPacketAsync(Packet packet);
    }
}
