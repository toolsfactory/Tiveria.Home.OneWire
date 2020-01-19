using System.IO;
using System.Threading.Tasks;
using Tiveria.Common.IO;

namespace Tiveria.Home.OneWire.OWServer.Messages
{
    public class ServerMessage : MessageBase
    {
        private int _payloadlen;
        private int _returnValue = 0;
        private Stream _stream;
        public int ReturnValue => _returnValue;

        public ServerMessage(Stream stream)
        {
            if (stream is null)
            {
                throw new System.ArgumentNullException(nameof(stream));
            }
            _stream = stream;
        }

        public Task ReadAndParseAsync()
        {
            return Task.Run(() =>
            {
                var reader = new EndianBinaryReader(_stream, Common.Endian.Big);
                ReadHeader(reader);
                ReadPayload(reader);
            });
        }

        private void ReadHeader(EndianBinaryReader reader)
        {
            _version = reader.ReadInt32();
            _payloadlen = reader.ReadInt32();
            _returnValue = reader.ReadInt32();
            _flags = (ControlFlags)reader.ReadInt32();
            _size = reader.ReadInt32();
            _offset = reader.ReadInt32();
        }

        private void ReadPayload(EndianBinaryReader reader)
        {
            if (_payloadlen > 0)
                _payload = reader.ReadBytes(_payloadlen);
        }

    }
}
