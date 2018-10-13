using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace NetworkPingScanner
{
    public class IpScanner
    {
        public async Task<IpScanResult> Scan(IPAddress ipAddress, int timeout)
        {
            using (var ping = new Ping())
            {
                var pingReply = await ping.SendPingAsync(ipAddress, timeout).ConfigureAwait(false);

                string dnsName = null;

                if (pingReply.Status == IPStatus.Success)
                {
                    try
                    {
                        var ipHostEntry = await Dns.GetHostEntryAsync(ipAddress).ConfigureAwait(false);
                        dnsName = ipHostEntry.HostName;
                    }
                    catch (SocketException)
                    {
                        // ignore this exception
                    }
                }

                return new IpScanResult(ipAddress, pingReply.Status == IPStatus.Success, dnsName);
            }
        }
    }
}