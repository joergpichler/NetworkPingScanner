using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace NetworkPingScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).Wait();
        }

        private static async Task MainAsync(string[] args)
        {
            await Parser.Default.ParseArguments<Options>(args)
                .MapResult(async (Options opts) => {
                    var tasks = new List<Task<(IPAddress, bool)>>();

                    Console.WriteLine("Pinging... " + opts.Timeout);

                    for (byte i = 1; i < 255; i++)
                    {
                        var ipAddress = new IPAddress(new byte[] { 192, 168, 0, i });
                        tasks.Add(Ping(ipAddress, opts.Timeout));
                    }

                    (IPAddress iPAddress, bool success)[] pingResults = await Task.WhenAll(tasks);

                    foreach (var pingResult in pingResults.Where(p => p.success))
                    {
                        Console.WriteLine($"* {pingResult.iPAddress}");
                    }
                }, errs => Task.FromResult(0));
        }

        private static async Task<(IPAddress, bool)> Ping(IPAddress iPAddress, int timeout)
        {
            using(var ping = new Ping())
            {
                var pingReply = await ping.SendPingAsync(iPAddress, timeout);
                return (iPAddress, pingReply.Status == IPStatus.Success);
            }
        }
    }
}
