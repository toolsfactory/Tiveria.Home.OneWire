using System.IO;
using Tiveria.Common.IO;

namespace Tiveria.Home.OneWire.OWServer.Messages
{
    public class ClientMessage : MessageBase
    {
        private MessageTypes _messageType = MessageTypes.Dir;
        public MessageTypes MessageType => _messageType;

        public ClientMessage(MessageTypes messageType, ControlFlags flags, byte[] payload = null) 
        {
            _flags = flags;
            if (payload == null)
                _payload = new byte[0];
            else
                _payload = payload;
            _messageType = messageType;
        }

        public void WriteToStream(Stream stream)
        {
            var writer = new EndianBinaryWriter(stream, Common.Endian.Big);
            WriteHeader(writer);
            WritePayload(writer);
        }
        public byte[] ToBuffer()
        {
            var memory = new MemoryStream();
            WriteToStream(memory);
            return memory.ToArray();
        }

        private void WriteHeader(EndianBinaryWriter writer)
        {
            writer.Write(Version);
            writer.Write(_payload.Length == 0 ? 0 : _payload.Length + 1);
            writer.Write((int)_messageType);
            writer.Write(((int)_flags) | 0x01);
            writer.Write(_size);
            writer.Write(0); // Offset always 0
        }

        private void WritePayload(EndianBinaryWriter writer)
        {
            if (_payload.Length > 0)
            {
                writer.Write(_payload);
                writer.Write((byte)0);
            }
        }
    }
}
