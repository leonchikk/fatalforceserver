using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using FatalForceServer.Engine.Extensions;
using FatalForceServer.Engine.Interfaces;
using FatalForceServer.Engine.Models;

namespace FatalForceServer.Engine
{
    public class SocketManager : ISocketManager
    {
        private Socket _socket;

        public void Configure(ServerConfig config)
        {
            _socket = SocketExtensions.Create(config)
                                      .Bind(config);
        }

        public async Task<SocketReceivedResult> ReceiveFromAsync()
        {
            byte[] _dataBuffer = new byte[256];
            EndPoint _remoteIp = new IPEndPoint(IPAddress.Any, 0);

            var receivedData = await _socket.ReceiveFromAsync(_dataBuffer, SocketFlags.None, _remoteIp);

            return new SocketReceivedResult()
            {
                ReceivedBytes = receivedData.ReceivedBytes,
                ReceivedData = _dataBuffer,
                RemoteEndPoint = receivedData.RemoteEndPoint
            };
        }

        public async Task SendAsync(byte[] data, ClientConnection recipient)
        {
            await _socket.SendAsync(data, recipient.EndPoint);
        }

        public async Task SendAsync(byte[] data, IEnumerable<ClientConnection> recipients)
        {
            foreach (var recipient in recipients)
            {
                await _socket.SendAsync(data, recipient.EndPoint);
            }
        }

        public async Task SendAsync(byte[] data, IEnumerable<ClientConnection> recipients, params ClientConnection[] except)
        {
            recipients = recipients.Except(except);

            foreach (var recipient in recipients)
            {
                await _socket.SendAsync(data, recipient.EndPoint);
            }
        }
    }
}
