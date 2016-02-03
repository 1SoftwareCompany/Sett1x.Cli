using CommandLine;
using CommandLine.Text;

namespace Elders.Pandora.Cli
{
    public class OpenOptions
    {
        public const string EnvVarOutput = "envvar";

        [Option('j', "jar", HelpText = "Input jar file")]
        public string Jar { get; set; }

        [Option('c', "cluster", HelpText = "Cluster name")]
        public string Cluster { get; set; }

        [Option('m', "machine", HelpText = "Machine name")]
        public string Machine { get; set; }

        [Option('a', "application", HelpText = "Application name")]
        public string Application { get; set; }

        [Option('o', "output", DefaultValue = EnvVarOutput, HelpText = "Output")]
        public string Output { get; set; }
    }
}
