using System;
using Tiveria.Home.OneWire.OWServer;
using Tiveria.Home.OneWire.OWServer.Messages;
using Tiveria.Common.Extensions;
using System.Text;
using System.Net.Sockets;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Tiveria.Home.OneWire.OWServer.Connection;

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
                       .AddFilter("SampleApp.Program", LogLevel.Trace)
                       .AddConsole();
            });
            var config = new ConnectionConfiguration("192.168.2.150", 4304);
            var connection = new OWServerConnection(loggerFactory, config);
            var manager = new OWServerManager(loggerFactory, connection);

            var result = await manager.DirAllAsync("/");
            Console.WriteLine($"Entries for / : {result.Length}");
            foreach (var item in result)
                Console.WriteLine($"   {item}");

            result = await manager.DirAllAsync("/28.FF9862921503");
            Console.WriteLine($"Entries for / : {result.Length}");
            foreach (var item in result)
                Console.WriteLine($"   {item}");
            var temp = await manager.ReadAsync("/28.FF9862921503/temperature");
            Console.WriteLine($"Entries for / : {temp}");
        }
    }
}
