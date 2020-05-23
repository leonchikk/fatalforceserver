using FatalForceServer.Engine.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FatalForceServer.Engine.Extensions
{
    public static class SocketExtensions
    {
        public static Socket Create(ServerConfig config)
        {
            int SIO_UDP_CONNRESET = -1744830452;

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Blocking = false;
            socket.IOControl(
                (IOControlCode)SIO_UDP_CONNRESET,
                new byte[] { 0, 0, 0, 0 },
                null
            );
            return socket;
        }

        public static Socket Bind(this Socket socket, ServerConfig config)
        {
            var bindingIP = new IPEndPoint(IPAddress.Parse(config.BindingIP), config.Port);
            socket.Bind(bindingIP);

            return socket;
        }

        public static async Task SendAsync(this Socket socket, byte[] data, EndPoint recipient)
        {
            var byteArray = new ArraySegment<byte>(data);

            await socket.SendToAsync(byteArray, SocketFlags.None, recipient);
        }
    }
}
