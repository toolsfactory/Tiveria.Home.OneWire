using System;
using System.Threading;
using System.Threading.Tasks;
using Tiveria.Home.OneWire.OWServer.Messages;

namespace Tiveria.Home.OneWire.OWServer
{
    public interface IOWServerConnection
    {
        bool Connected { get; }
        Guid ConnectionId { get; }
        string HostName { get; }
        int Port { get; }

        Task ConnectAsync();
        Task DisconnectAsync();
        ValueTask DisposeAsync();
        Task<(ServerMessageHeader Header, byte[] Payload)> ReceiveAsync(CancellationToken token = default);
        Task SendAsync(ClientMessageHeader header, byte[] payload, CancellationToken token = default);
    }
}