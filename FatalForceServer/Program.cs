using FatalForceServer.Engine;
using FatalForceServer.Engine.Interfaces;

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
                    config.AllowedClientTimeOut = 2000;
                    config.CheckClientsAvailableFrequency = 100;
                })
                .ConfigureServices(services =>
                {
                    services.RegisterSingleton<IConnectionManager, ConnectionManager>();
                    services.RegisterSingleton<ISocketManager, SocketManager>();
                })
                .Build();

            server.Run();
        }
    }
}
