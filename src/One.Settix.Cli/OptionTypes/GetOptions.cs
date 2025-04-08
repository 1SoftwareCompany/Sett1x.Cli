using CommandLine;

namespace One.Settix.Cli.Core.OptionTypes
{
    [Verb("get", HelpText = "Gets a specific key.")]
    public class GetOptions
    {
        public const string EnvVarOutput = "envvar";
        public const string ConsulOutput = "consul";

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

        [Option('k', "key", HelpText = "The key")]
        public string Key { get; set; }
    }
}
