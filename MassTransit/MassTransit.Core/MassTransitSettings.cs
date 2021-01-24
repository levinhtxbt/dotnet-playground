namespace MassTransit.Core
{
    public class MassTransitSettings
    {
        public string AWSMQ { get; set; }

        public string Host { get; set; }

        public string VirtualHost { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public ushort PrefetchCount { get; set; }
    }
}