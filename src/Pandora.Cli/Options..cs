using CommandLine;
using CommandLine.Text;

namespace Elders.Pandora.Cli
{
    public class Options
    {
        public Options()
        {
            OpenVerb = new OpenOptions();
        }

        [VerbOption("open", HelpText = "Opens the pandora box.")]
        public OpenOptions OpenVerb { get; set; }

        [ParserState]
        public IParserState LastParserState { get; set; }

        [HelpOption]
        public string GetUsage(string verb = null)
        {
            return HelpText.AutoBuild(this, verb);
        }
    }
}
