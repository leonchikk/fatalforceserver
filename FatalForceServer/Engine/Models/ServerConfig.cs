namespace FatalForceServer.Engine.Models
{
    public class ServerConfig
    {
        public string BindingIP { get; set; }
        public int Port { get; set; }
        public long AllowedClientTimeOut { get; set; } = 2000;
        public int CheckClientsAvailableFrequency { get; set; } = 500;
        public int Rate { get; set; } = 33;
        public int MaxAllowedPing { get; set; } = 300;
    }
}
