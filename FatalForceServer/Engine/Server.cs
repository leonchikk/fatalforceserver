using FatalForceServer.Core;
using FatalForceServer.Core.Packets;
using FatalForceServer.Engine.Extensions;
using FatalForceServer.Engine.Interfaces;
using FatalForceServer.Engine.Models;
using FatalForceServer.Logic;
using SimpleInjector;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace FatalForceServer
{
    public class Server
    {
        private readonly ISocketManager _socketManager;
        private readonly IClientManager _clientManager;
        private readonly IGameStateManager _gameStateManager;
        private readonly IGameProcessManager _gameProcessManager;

        private readonly ConcurrentQueue<Packet> _queue;
        private readonly ServerConfig _config;

        public Server(Container container, ServerConfig config)
        {
            Log.Info($"Starting the server on {config.Port} port with rate {config.Rate}");

            _queue = new ConcurrentQueue<Packet>();

            _socketManager = container.GetInstance<ISocketManager>();
            _clientManager = container.GetInstance<IClientManager>();
            _gameStateManager = container.GetInstance<IGameStateManager>();
            _gameProcessManager = container.GetInstance<IGameProcessManager>();

            _config = config;

            Log.Info($"Configuring...");

            _socketManager.Configure(config);

            Log.Info($"Done");
        }

        public void Run()
        {
            Task listener = Task.Run(async () => await ListenAsync());
            Task processor = Task.Run(async () => await ProcessAsync());
            Task pingClients = Task.Run(async () => await CheckClientsAvailable());

            Log.Info($"Starting listening and processing incoming data.....");

            Task.WaitAll(listener, processor, pingClients);
        }

        private async Task ProcessAsync()
        {
            while (true)
            {
                try
                {
                    await Task.Delay(1000 / _config.Rate);

                    while (_queue.TryDequeue(out Packet queueItem))
                    {
                        await _gameProcessManager.ProcessIncomingPacketAsync(queueItem);
                    }

                    await _gameStateManager.SendWorldStateToClients();
                }
                catch (Exception ex) //TODO Rewrite this on that case if here's will be an aggregate exception
                {
                    Log.Error($"{ex.Message}\n{ex.StackTrace}");
                }
            }
        }

        private async Task CheckClientsAvailable()
        {
            while (true)
            {
                var notAvailableClients = _clientManager.GetNotAvailableClients(_config.AllowedClientTimeOut);

                await _clientManager.DisconnectAsync(notAvailableClients, "Connection timeout");

                _gameStateManager.RemovePlayers(notAvailableClients.Select(client => client.Id));

                await Task.Delay(_config.CheckClientsAvailableFrequency);
            }
        }

        private async Task ListenAsync()
        {
            while (true)
            {
                try
                {
                    var receivedData = await _socketManager.ReceiveFromAsync();

                    _queue.Enqueue(receivedData.ReceivedData.ConvertToPacket(receivedData));
                }
                catch (Exception ex)
                {
                    Log.Error($"{ex.Message}\n{ex.StackTrace}");
                }
            }
        }
    }
}
