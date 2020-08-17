using CommandLine;
using System;

namespace Pandora.Cli.Core.OptionTypes
{
    [Verb("validate", HelpText = "Validates a pandora root jar configuration file")]
    public class ValidateOptions
    {
        [Option('f', "fileName", Required = true, HelpText = "The name of the JSON file you want to validate. Use only if you want to validate a seperate file, not all in the derictory")]
        public string FileName { get; set; }

        [Option('p', "path", Required = false, HelpText = "Absolute path to the directory where pandora configurations are. Default would be the root directory of execution.")]
        public string Path { get; set; } = Environment.CurrentDirectory;
    }
}
