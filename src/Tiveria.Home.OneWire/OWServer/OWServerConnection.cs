using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tiveria.Home.OneWire.OWServer.Messages;


namespace Tiveria.Home.OneWire.OWServer
{

    public class OWServerConnection : IAsyncDisposable, IOWServerConnection
    {
        #region Default constants
        public static readonly TimeSpan DefaultServerTimeout = TimeSpan.FromSeconds(8);
        #endregion

        #region private fields
        private readonly ILoggerFactory _loggerFactory;
        private readonly IConnectionConfiguration _config;
        private readonly ILogger _logger;
        private readonly int _port;
        private readonly string _hostName;
        private bool _disposed;
        private Socket _socket;
        private NetworkStream _stream;
        private Guid _connectionId;
        #endregion

        #region Properties
        public string HostName => _hostName;
        public int Port => _port;
        public bool Connected { get { return (_socket == null) ? false : _socket.Connected; } }
        public Guid ConnectionId => _connectionId;
        #endregion

        public OWServerConnection(ILoggerFactory loggerFactory, IConnectionConfiguration config)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _logger = _loggerFactory.CreateLogger<OWServerConnection>();
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task ConnectAsync()
        {
            CheckDisposed();
            if (Connected)
                await DisconnectAsync();
            _connectionId = Guid.NewGuid();
            using (_logger.BeginScope(_connectionId))
            {
                _logger.LogDebug("Initiating new connection");
                await InternalCreateAndConnectSocketAsync();
                _stream = new NetworkStream(_socket, System.IO.FileAccess.ReadWrite);
                _logger.LogDebug("New connection established");
            }
        }

        private async Task InternalCreateAndConnectSocketAsync()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Blocking = true;
            _socket.ReceiveTimeout = 10000;
            await _socket.ConnectAsync(_config.Host, _config.Port);
        }

        public Task DisconnectAsync()
        {
            CheckDisposed();
            return Task.Run(() =>
            {
                if (!Connected)
                    return;
                _logger.LogDebug("Disconnecting");
                using (_logger.BeginScope(_connectionId))
                {
                    _stream.Close();
                    _socket.Disconnect(true);
                }
                _connectionId = Guid.Empty;
            });
        }

        public async Task SendAsync(ClientMessageHeader header, byte[] payload, CancellationToken token = default)
        {
            if (!Connected)
                throw new InvalidOperationException("Cannot send. No open connection!");
            var buffer = header.ToByteArray();
            await _stream.WriteAsync(buffer, 0, buffer.Length, token);
            if (payload != null && payload.Length > 0)
                await _stream.WriteAsync(payload, 0, payload.Length, token);
            await _stream.FlushAsync(token);
        }

        public async Task<(ServerMessageHeader Header, byte[] Payload)> ReceiveAsync(CancellationToken token = default)
        {
            if (!Connected)
                throw new InvalidOperationException("Cannot send. No open connection!");
            var header = await ReadServerMessageHeader(token);
            var payload = await ReadServerPayload(header, token);
            return (header, payload);
        }

        private async Task<byte[]> ReadServerPayload(ServerMessageHeader header, CancellationToken token)
        {
            var payload = new byte[header.PayloadLength];
            if (header.PayloadLength > 0)
            {
                var payloadsize = await _stream.ReadAsync(payload, token);
            }
            return payload;
        }

        private async Task<ServerMessageHeader> ReadServerMessageHeader(CancellationToken token)
        {
            var headerbuffer = new byte[ServerMessageHeader.HeaderSize];
            var headersize = await _stream.ReadAsync(headerbuffer, token);
            var header = new ServerMessageHeader(headerbuffer);
            return header;
        }

        private void CheckDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(OWServerConnection));
            }
        }


        public async ValueTask DisposeAsync()
        {
            if (!_disposed)
            {
                using (_logger.BeginScope(_connectionId))
                {
                    _logger.LogDebug("Disposing resources");
                    await DisconnectAsync();
                    await _stream.DisposeAsync();
                    _socket.Dispose();
                }
            }
        }
    }
}
