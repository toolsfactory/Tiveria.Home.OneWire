using System;
using Tiveria.Home.OneWire.OWServer;
using Tiveria.Home.OneWire.OWServer.Messages;
using Tiveria.Common.Extensions;
using System.Text;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Tiveria.Home.OneWire.SampleApp
{
    class Program
    {

        static async Task Main(string[] args)
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddFilter("Microsoft", LogLevel.Warning)
                       .AddFilter("System", LogLevel.Warning)
                       .AddFilter("SampleApp.Program", LogLevel.Debug)
                       .AddConsole();
            });
            var config = new ConnectionConfiguration("192.168.2.150", 4304);
            var connection = new OWServerConnection(loggerFactory, config);
            var payload = "/".ToNullTerminatedBytes();
            var dirallmessageheader = new ClientMessageHeader(MessageTypes.DirAll, ControlFlags.Default, payload);

            await connection.ConnectAsync();
            await connection.SendAsync(dirallmessageheader, payload);
            var result = await connection.ReceiveAsync();
            await connection.DisconnectAsync();
            if (result.Payload != null)
            {
                var value = result.Payload.FromNullTerminatedBytes();
                Console.WriteLine(value);
            }
        }

        static void OldMain()
        {
            var owsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            owsocket.Blocking = true;
            owsocket.ReceiveTimeout = 10000;
            owsocket.Connect("192.168.2.150", 4304);
            var netStream = new NetworkStream(owsocket, System.IO.FileAccess.ReadWrite);
            var msg = new ClientMessage(MessageTypes.DirAll, ControlFlags.OWNet | ControlFlags.Persistence | ControlFlags.BusRet, Encoding.ASCII.GetBytes("/"));
            var bytes = msg.ToBuffer();
            Console.WriteLine(bytes.ToHexView());

            var head = new ClientMessageHeader(MessageTypes.DirAll, ControlFlags.Default, Encoding.ASCII.GetBytes("/"));
            var buf = new byte[ClientMessageHeader.HeaderSize];
            head.WriteTo(buf);
            Console.WriteLine(buf.ToHexView());
            msg.WriteToStream(netStream);
            netStream.Flush();
            var servermsg = new ServerMessage(netStream);
            Console.WriteLine("Now Reading");
            servermsg.ReadAndParseAsync().Wait();
            if (servermsg.PayLoad != null)
            {
                Console.WriteLine(new String(Encoding.ASCII.GetChars(servermsg.PayLoad)));
            }
            owsocket.Close();

        }
    }

    public static class Extensions 
    { 
        public static string FromNullTerminatedBytes(this byte[] data)
        {
            if (data is null)
            {
                throw new ArgumentNullException(nameof(data));
            }
            var result = new String(Encoding.ASCII.GetChars(data));
            return result.TrimEnd('\0');
        }

        public static byte[] ToNullTerminatedBytes(this string value)
        {
            return Encoding.ASCII.GetBytes(value+"\x00");
        }
    }
}
