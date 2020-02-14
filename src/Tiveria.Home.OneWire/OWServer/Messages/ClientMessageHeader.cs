using System;
using System.Collections.Generic;
using System.Text;

namespace Tiveria.Home.OneWire.OWServer.Messages
{
    /// <summary>
    /// Header of of messages sent from a client to OWServer
    /// <code>
    /// +-----------------------------------------------------------------------------+
    /// |          Fixed Header                                                       |
    /// +------------+------------+------------+------------+------------+------------+
    /// | byte 0-3   | byte 4-7   | byte 8-11  | byte 11-15 | byte 16-19 | byte 20-23 |
    /// +------------+------------+------------+------------+------------+------------+
    /// | Version    | Payl.Length| Type       | Ctrl.Flags | Size       | Offset     |
    /// +------------+------------+------------+------------+------------+------------+
    /// </code>
    /// </summary>
    public struct ClientMessageHeader
    {
        public const uint ProtocolVersion = 0;
        public const uint HeaderSize = 6 * sizeof(uint);
        public const int DefaultDataSize = 4096;
        public uint Version => ClientMessageHeader.ProtocolVersion;
        public readonly uint PayloadLength { get; }
        public readonly uint Size { get; }
        public readonly MessageTypes MessageType { get; }
        public readonly ControlFlags Flags { get; }
        public readonly uint Offset  => 0;

        public ClientMessageHeader(MessageTypes messageType, ControlFlags flags, byte[] payload = null, uint size=0) : this()
        {
            if(payload == null || payload.Length == 0)
                PayloadLength = 0;
            else
                PayloadLength = (uint)payload.Length;
            Size = size;
            MessageType = messageType;
            Flags = flags;
        }

        public byte[] ToByteArray()
        {
            var buffer = new byte[HeaderSize];
            WriteTo(buffer);
            return buffer;
        }

        public void WriteTo(byte[] buffer, int index = 0)
        {
            if (buffer is null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }
            if (buffer.Length - index < HeaderSize)
            {
                throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer too small or wrong index");
            }
            WriteIntBE(buffer,  0, Version);
            WriteIntBE(buffer,  4, PayloadLength);
            WriteIntBE(buffer,  8, (uint) MessageType);
            WriteIntBE(buffer, 12, (uint) Flags);
            WriteIntBE(buffer, 16, Size);
            WriteIntBE(buffer, 20, Offset);
        }

        private static void WriteIntBE(byte[] buffer, int index, uint value)
        {
            var data = BitConverter.GetBytes(value);
            if (BitConverter.IsLittleEndian)
            {
                buffer[index] = data[0+3];
                buffer[index+1] = data[0+2];
                buffer[index+2] = data[0+1];
                buffer[index+3] = data[0];
            } 
            else
            {
                buffer[index] = data[0];
                buffer[index + 1] = data[0 + 1];
                buffer[index + 2] = data[0 + 2];
                buffer[index + 3] = data[0 + 3];
            }
        }
    }

}