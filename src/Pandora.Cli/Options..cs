using CommandLine;
using CommandLine.Text;

namespace Elders.Pandora.Cli
{
    public class Options
    {
        public Options()
        {
            OpenVerb = new OpenOptions();
            GetVerb = new GetOptions();
        }

        [VerbOption("get", HelpText = "Opens the pandora box.")]
        public GetOptions GetVerb { get; set; }

        [VerbOption("open", HelpText = "Opens the pandora box.")]
        public OpenOptions OpenVerb { get; set; }

        [VerbOption("validate", HelpText = "Validates a pandora root jar configuration file")]
        public ValidateOptions ValidateVerb { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage(string verb = null)
        {
            return HelpText.AutoBuild(this, verb);
        }
    }
}
