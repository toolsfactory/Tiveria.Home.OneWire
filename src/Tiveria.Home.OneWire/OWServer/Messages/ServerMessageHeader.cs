using System;

namespace Tiveria.Home.OneWire.OWServer.Messages
{
    /// <summary>
    /// <code>
    /// +-----------------------------------------------------------------------------+
    /// |          Fixed Header                                                       |
    /// +------------+------------+------------+------------+------------+------------+
    /// | byte 0-3   | byte 4-7   | byte 8-11  | byte 11-15 | byte 16-19 | byte 20-23 |
    /// +------------+------------+------------+------------+------------+------------+
    /// | Version    | Payl.Length| Type       | Ctrl.Flags | Size       | Offset     |
    /// +------------+------------+------------+------------+------------+------------+
    /// </code>
    /// 
    /// </summary>
    public struct ServerMessageHeader
    {
        public static readonly uint ProtocolVersion = 0;
        public static readonly uint HeaderSize = 6 * sizeof(uint);
        private uint _payloadLength;
        private uint _size;
        private int _returnValue;
        private ControlFlags _flags;
        private uint _offset;
        private uint _version;

        public uint Version => _version;
        public readonly uint PayloadLength => _payloadLength;
        public readonly uint Size => _size;
        public readonly int ReturnValue => _returnValue;
        public readonly ControlFlags Flags => _flags;
        public readonly uint Offset => _offset;

        public ServerMessageHeader(byte[] buffer, int index = 0) : this()
        {
            if (buffer is null)
            {
                throw new ArgumentNullException(nameof(buffer));
            }
            if (buffer.Length - index < HeaderSize)
            {
                throw new ArgumentOutOfRangeException(nameof(buffer), "Buffer too small or wrong index");
            }
            _version = ReadUIntBE(buffer, 0);
            _payloadLength = ReadUIntBE(buffer, 4);
            _returnValue = (int) ReadUIntBE(buffer, 8);
            _flags = (ControlFlags) ReadUIntBE(buffer, 12);
            _size = ReadUIntBE(buffer, 16);
            _offset = ReadUIntBE(buffer, 20);
        }

        private static uint ReadUIntBE(byte[] buffer, int index)
        {
            var data = new byte[4];
            if (BitConverter.IsLittleEndian)
            {
                data[3] = buffer[index];
                data[2] = buffer[index + 1];
                data[1] = buffer[index + 2];
                data[0] = buffer[index + 3];
            }
            else
            {
                data[0] = buffer[index];
                data[1] = buffer[index + 1];
                data[2] = buffer[index + 2];
                data[3] = buffer[index + 3];
            }
            return BitConverter.ToUInt32(data);
        }
    }

}