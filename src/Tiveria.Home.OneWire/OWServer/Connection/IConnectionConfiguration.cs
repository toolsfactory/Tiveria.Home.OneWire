using Tiveria.Common.Extensions;

namespace Tiveria.Home.OneWire.OWServer
{
    public interface IConnectionConfiguration
    {
        string Host { get; }
        int Port { get; }
    }
}
