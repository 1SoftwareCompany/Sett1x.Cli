using CommandLine;
using Pandora.Cli.Core.OptionTypes;
using System;
using System.Threading.Tasks;

namespace Pandora.Cli.Core
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                PandoraRunner pandoraRunner = new PandoraRunner();

                await Parser.Default
                     .ParseArguments<OpenOptions, GetOptions, ValidateOptions>(args)
                     .MapResult<OpenOptions, GetOptions, ValidateOptions, Task>(
                         async (OpenOptions opts) => await pandoraRunner.OpenCommandAsync(opts),
                         async (GetOptions opts) => await pandoraRunner.GetCommandAsync(opts),
                         (ValidateOptions opts) => Task.FromResult(pandoraRunner.ValidateCommand(opts)),
                         errors => Task.FromResult("Could not set the variable!")
                     );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
