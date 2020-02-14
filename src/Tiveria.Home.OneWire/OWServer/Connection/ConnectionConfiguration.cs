using System;
using Tiveria.Common.Extensions;

namespace Tiveria.Home.OneWire.OWServer.Connection
{
    public class ConnectionConfiguration : IConnectionConfiguration
    {
        public const int DefaultPort = 4304;
        public const string DefaultHost = "localhost";
        public ConnectionConfiguration(string host = DefaultHost, int port = DefaultPort)
        {
            Host = host ?? throw new ArgumentNullException(nameof(host));
            if(!Host.IsValidHostName())
                throw new ArgumentException("No valid hostname", nameof(host));
            Port = port;
        }

        public string Host { get; }

        public int Port { get; }
    }
}
