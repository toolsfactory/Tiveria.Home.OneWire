using System.Collections.Generic;
using System.IO;
using System.Text;
using Tiveria.Common.IO;

namespace Tiveria.Home.OneWire.OWServer.Messages
{
    /// <summary>
    /// <code>
    /// +-----------------------------------------------------------------------------+----------------+
    /// |          Fixed Header                                                       | Payload        |
    /// +------------+------------+------------+------------+------------+------------+----------------+
    /// | byte 0-3   | byte 4-7   | byte 8-11  | byte 11-15 | byte 16-19 | byte 20-23 | byte 24-xxx    |
    /// +------------+------------+------------+------------+------------+------------+----------------+
    /// | Version    | Payl.Length| Type       | Ctrl.Flags | Size       | Offset     | Raw payload    |
    /// +------------+------------+------------+------------+------------+------------+----------------+
    /// </code>
    /// 
    /// </summary>
    public abstract class MessageBase
    {
        public static readonly int ProtocolVersion = 0;

        protected int _version = MessageBase.ProtocolVersion;
        protected int _size;
        protected int _offset;
        protected ControlFlags _flags;
        protected byte[] _payload;

        public int Version => _version;
        public int PayloadLength => _payload.Length;
        public int Size => _size;
        public ControlFlags Flags => _flags;
        public int Offset => _offset;
        public byte[] PayLoad => _payload;
    }
}
