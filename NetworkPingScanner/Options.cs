using CommandLine;

namespace NetworkPingScanner
{
    /// <summary>
    /// Command line options for startup
    /// </summary>
    public class Options
    {
        [Option('t', "timeout", Default = 1000, HelpText = "Ping timeout")]
        public int Timeout { get; set; }
    }
}
