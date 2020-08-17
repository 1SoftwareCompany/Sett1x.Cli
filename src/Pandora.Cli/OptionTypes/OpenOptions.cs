using CommandLine;

namespace Pandora.Cli.Core.OptionTypes
{
    [Verb("open", HelpText = "Opens the pandora box.")]
    public class OpenOptions
    {
        public const string EnvVarOutput = "envvar";
        public const string ConsulOutput = "consul";

        [Option('j', "jar", HelpText = "Input jar file")]
        public string Jar { get; set; }

        [Option('c', "cluster", HelpText = "Cluster name")]
        public string Cluster { get; set; }

        [Option('m', "machine", HelpText = "Machine name")]
        public string Machine { get; set; }

        [Option('a', "application", HelpText = "Application name")]
        public string Application { get; set; }

        [Option('o', "output", Default = EnvVarOutput, HelpText = "Output")]
        public string Output { get; set; }

        [Option('h', "consulhost", HelpText = "ConsulHost")]
        public string ConsulHost { get; set; }
    }
}
