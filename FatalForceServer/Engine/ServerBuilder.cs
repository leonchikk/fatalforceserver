using FatalForceServer.Engine.Models;
using Microsoft.Extensions.Configuration;
using SimpleInjector;
using System;

namespace FatalForceServer
{
    public class ServerBuilder
    {
        private readonly Container _container;
        private readonly ConfigurationBuilder _configurationBuilder;

        private ServerConfig _serverConfig;

        public ServerBuilder()
        {
            _container = new Container();
            _configurationBuilder = new ConfigurationBuilder();

            _serverConfig = new ServerConfig();
        }

        public ServerBuilder ConfigureServices(Action<Container> callback)
        {
            callback(_container);

            return this;
        }

        public ServerBuilder ConfigureServer(Action<ServerConfig> callback)
        {
            callback(_serverConfig);

            return this;
        }

        public Server Build()
        {
            return new Server(_container, _serverConfig);
        }
    }
}
