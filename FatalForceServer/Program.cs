using FatalForceServer.Engine;
using FatalForceServer.Engine.Interfaces;
using FatalForceServer.Logic;

namespace FatalForceServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new ServerBuilder()
                .ConfigureServer(config =>
                {
                    config.BindingIP = "127.0.0.1";
                    config.Port = 27015;
                    config.AllowedClientTimeOut = 3000;
                    config.CheckClientsAvailableFrequency = 100;
                    config.Rate = 100;
                })
                .ConfigureServices(services =>
                {
                    services.RegisterSingleton<IConnectionManager, ConnectionManager>();
                    services.RegisterSingleton<ISocketManager, SocketManager>();
                    services.RegisterSingleton<IClientManager, ClientManager>();
                    services.RegisterSingleton<IGameStateManager, GameStateManager>();
                    services.RegisterSingleton<IGameProcessManager, GameProcessManager>();
                })
                .Build();

            server.Run();
        }
    }
}
