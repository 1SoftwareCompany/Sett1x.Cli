using CommandLine;
using CommandLine.Text;

namespace One.Settix.Cli.Core.OptionTypes
{
    public class Options
    {
        public Options()
        {
            OpenVerb = new OpenOptions();
            GetVerb = new GetOptions();
        }

        public GetOptions GetVerb { get; set; }

        public OpenOptions OpenVerb { get; set; }

        public ValidateOptions ValidateVerb { get; set; }
        //public string GetUsage(string verb = null)
        //{
        //    return HelpText.AutoBuild(this, verb);
        //} // temporarily check 
    }
}
