using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Tiveria.Home.OneWire.OWServer.Messages;
using Tiveria.Home.OneWire.OWServer.Connection;
using Tiveria.Common.Extensions;

namespace Tiveria.Home.OneWire.OWServer
{
    public class OWServerManager
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IOWServerConnection _connection;

        public OWServerManager(ILoggerFactory loggerFactory, IOWServerConnection connection)
        {
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task<string[]> DirAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("invalid path", nameof(path));
            }

            var payload = path.ToNullTerminatedBytes();
            var messageheader = new ClientMessageHeader(MessageTypes.Dir, ControlFlags.Default, payload);

            return await InternalDirAsync(payload, messageheader);
        }

        public async Task<string[]> DirAllAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException("invalid path", nameof(path));
            }

            var payload = path.ToNullTerminatedBytes();
            var messageheader = new ClientMessageHeader(MessageTypes.DirAll, ControlFlags.Default | ControlFlags.BusRet, payload);
            var buffer = messageheader.ToByteArray();
            return await InternalDirAsync(payload, messageheader);
        }


        private async Task<string[]> InternalDirAsync(byte[] payload, ClientMessageHeader messageheader)
        {
            await _connection.ConnectAsync();
            await _connection.SendAsync(messageheader, payload);
            var result = await _connection.ReceiveAsync();
            await _connection.DisconnectAsync();
            if (result.Payload != null && result.Payload.Length > 0)
            {
                var value = result.Payload.FromNullTerminatedBytes();
                var folders = value.Split(',');
                return folders;
            }
            return new string[0];
        }

        public async Task<string> ReadAsync(string path)
        {
            var result = await InternalReadRawAsync(path, ClientMessageHeader.DefaultDataSize);
            return result.FromNullTerminatedBytes();
        }

        private async Task<byte[]> InternalReadRawAsync(string path, uint datalen)
        {
            await _connection.ConnectAsync();
            var payload = path.ToNullTerminatedBytes();
            var messageheader = new ClientMessageHeader(MessageTypes.Read, ControlFlags.Default, payload, datalen);
            await _connection.SendAsync(messageheader, payload);
            var result = await _connection.ReceiveAsync();
            await _connection.DisconnectAsync();
            if (result.Payload != null && result.Payload.Length > 0)
            {
                var value = result.Payload.FromNullTerminatedBytes();
            }
            return result.Payload;

        }
    }
}
