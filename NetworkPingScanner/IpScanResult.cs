using System;
using System.Net;

namespace NetworkPingScanner
{
    public class IpScanResult
    {
        public IpScanResult(IPAddress ipAddress, bool success) : this(ipAddress, success, null)
        {
        }

        public IpScanResult(IPAddress ipAddress, bool success, string dnsName)
        {
            IpAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
            Success = success;
            DnsName = dnsName;
        }

        public IPAddress IpAddress { get; }

        public bool Success { get; }

        public string DnsName { get; }
    }
}