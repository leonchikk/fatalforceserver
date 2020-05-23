using FatalForceServer.Core;
using FatalForceServer.Core.Packets;
using FatalForceServer.Engine.Extensions;
using FatalForceServer.Engine.Interfaces;
using FatalForceServer.Engine.Models;
using SimpleInjector;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace FatalForceServer
{
    public class Server
    {
        private readonly ISocketManager _socketManager;
        private readonly IConnectionManager _connectionManager;
        private readonly ConcurrentQueue<Packet> _queue;

        private readonly int _checkClientsAvailableFrequency;
        private readonly long _allowedClientTimeOut;
        private readonly int _updateRate;

        public Server(Container container, ServerConfig config)
        {
            Log.Info($"Starting the server on {config.Port} port");

            _socketManager = container.GetInstance<ISocketManager>();
            _connectionManager = container.GetInstance<IConnectionManager>();

            _queue = new ConcurrentQueue<Packet>();
            _checkClientsAvailableFrequency = config.CheckClientsAvailableFrequency;
            _allowedClientTimeOut = config.AllowedClientTimeOut;
            _updateRate = config.Rate;

            Log.Info($"Configuring...");

            _socketManager.Configure(config);

            Log.Info($"Done");
        }

        public void Run()
        {
            Task listener = Task.Run(async () => await ListenAsync());
            Task processor = Task.Run(async () => await ProcessAsync());
            Task pingClients = Task.Run(async () => await CheckClientsAvailable());

            Task.WaitAll(listener, processor, pingClients);
        }

        private async Task ProcessAsync()
        {
            while (true)
            {
                try
                {
                    /////////////////////////////
                    while (_queue.TryDequeue(out Packet queueItem))
                    {
                        if (queueItem is ConnectionPacket)
                        {
                            var connectionPacket = queueItem as ConnectionPacket;
                            var acceptPacket = new AcceptConnectionPacket();

                            var clientId = _connectionManager.AddConnection(connectionPacket);
                            var connectedClient = _connectionManager.GetClientById(clientId);

                            await _socketManager.SendAsync(acceptPacket.SetIdentifier(clientId)
                                                                       .Serialize(),
                                                           connectedClient);

                            await _socketManager.SendAsync(connectionPacket
                                                                        .SetIdentifier(clientId)
                                                                        .Serialize(),
                                recipients: _connectionManager.GetAllAvailableRecipients(),
                                except:     connectedClient
                             );
                        }

                        if (queueItem is PingPacket)
                        {
                            var pingPacket = queueItem as PingPacket;

                            _connectionManager.SetAsPinged(pingPacket.ClientId);
                        }

                    }
                    ///////////////////////////////


                    await PingClients();
                    await Task.Delay(1000 / _updateRate);

                }
                catch (Exception ex) //TODO Rewrite this on that case if here's will be an aggregate exception
                {
                    Log.Error($"{ex.Message}\n{ex.StackTrace}");
                }
            }
        }

        private async Task PingClients()
        {
            var pingPacket = new PingPacket();
            await _socketManager.SendAsync(
                            data:        pingPacket.Serialize(),
                            recipients: _connectionManager.GetAllAvailableRecipients()
                        );
        }

        private async Task CheckClientsAvailable()
        {
            while (true)
            {
                var recipients = _connectionManager.GetAllAvailableRecipients();

                foreach (var recipient in recipients)
                {
                    var clientTimeSpanFromLastPing = DateTime.UtcNow - (new DateTime(recipient.LastPingTimeStamp));

                    if (clientTimeSpanFromLastPing.TotalMilliseconds >= _allowedClientTimeOut)
                    {
                        _connectionManager.Disconnect(recipient.Id);

                        var disconnectPacket = new DisconnectPacket(recipient.Id, "Connection timeout");

                        await _socketManager.SendAsync(
                                data:        disconnectPacket.Serialize(),
                                recipients: _connectionManager.GetAllAvailableRecipients()
                             );
                    }
                }

                await Task.Delay(_checkClientsAvailableFrequency);
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
