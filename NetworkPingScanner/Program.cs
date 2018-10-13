using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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
                    var tasks = new List<Task<IpScanResult>>();

                    Console.WriteLine("Pinging... " + opts.Timeout);

                    for (byte i = 1; i < 255; i++)
                    {
                        var ipAddress = new IPAddress(new byte[] { 192, 168, 0, i });
                        tasks.Add(new IpScanner().Scan(ipAddress, opts.Timeout));
                    }

                    IpScanResult[] pingResults = await Task.WhenAll(tasks);

                    foreach (var pingResult in pingResults.Where(p => p.Success))
                    {
                        Console.WriteLine($"* {pingResult.IpAddress} {pingResult.DnsName}");
                    }
                }, errs => Task.FromResult(0));
        }
    }
}
