using CommandLine;
using One.Settix.Cli.Core.OptionTypes;
using System;
using System.Threading.Tasks;

namespace One.Settix.Cli.Core
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            try
            {
                SettixRunner settixRunner = new SettixRunner();

                await Parser.Default
                     .ParseArguments<OpenOptions, GetOptions, ValidateOptions>(args)
                     .MapResult<OpenOptions, GetOptions, ValidateOptions, Task>(
                         async (OpenOptions opts) => await settixRunner.OpenCommandAsync(opts),
                         async (GetOptions opts) => await settixRunner.GetCommandAsync(opts),
                         (ValidateOptions opts) => Task.FromResult(settixRunner.ValidateCommand(opts)),
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
