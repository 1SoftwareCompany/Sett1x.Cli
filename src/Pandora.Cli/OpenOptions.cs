using CommandLine;
using System;

namespace Elders.Pandora.Cli
{
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

        [Option('o', "output", DefaultValue = EnvVarOutput, HelpText = "Output")]
        public string Output { get; set; }

        [Option('h', "consulhost", HelpText = "ConsulHost")]
        public string ConsulHost { get; set; }
    }

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

        [Option('o', "output", DefaultValue = EnvVarOutput, HelpText = "Output")]
        public string Output { get; set; }

        [Option('h', "consulhost", HelpText = "ConsulHost")]
        public string ConsulHost { get; set; }

        [Option('k', "key", HelpText = "The key")]
        public string Key { get; set; }
    }

    public class ValidateOptions
    {
        [Option('f', "fileName", Required = true, HelpText = "The name of the JSON file you want to validate. Use only if you want to validate a seperate file, not all in the derictory")]
        public string FileName { get; set; }

        [Option('p', "path", Required = false, HelpText = "Absolute path to the directory where pandora configurations are. Default would be the root directory of execution.")]
        public string Path { get; set; } = Environment.CurrentDirectory;
    }
}
