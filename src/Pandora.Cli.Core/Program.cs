using CommandLine;
using Elders.Pandora;
using Elders.Pandora.Box;
using Newtonsoft.Json;
using Pandora.Cli.Core.OptionTypes;
using System;
using System.ComponentModel;
using System.IO;

namespace Pandora.Cli.Core
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    // Console.WriteLine(options.GetUsage());
                    return 0;
                }

                var zz = CommandLine.Parser.Default.ParseArguments<OpenOptions, GetOptions, ValidateOptions>(args).MapResult(
                    (OpenOptions opts) => OpenComand(opts),
                    (GetOptions opts) => GetCommand(opts),
                    (ValidateOptions opts) => ValidateCommand(opts),
                    _ => "Could not set the variable!"
                    );


            }
            catch (Exception ex)
            {
                // log.ErrorException(ex.Message, ex);
                return 1;
            }

            return 0;
        }

        private static string ValidateCommand(object invokedVerbInstance)
        {
            try
            {
                var validateOptions = (ValidateOptions)invokedVerbInstance;
                var fileName = validateOptions.FileName + ".json";

                // log.Info($"STARTED -> validating '{fileName}' in directory '{validateOptions.Path}'");

                Validator.Validator.Validate(validateOptions.Path, fileName);

                // log.Info($"FINISHED -> validating '{fileName}' in directory '{validateOptions.Path}'");
                return "Successfully validated!";
            }
            catch (Exception ex)
            {

                return $"Validation failed: `{ex.Message}`";
            }
        }

        private static string GetCommand(object invokedVerbInstance)
        {
            var getOptions = (GetOptions)invokedVerbInstance;

            string applicationName = getOptions.Application;
            string cluster = getOptions.Cluster;
            string machine = getOptions.Machine;
            if (getOptions.Output == GetOptions.ConsulOutput)
            {
                Uri consulAddress = null;
                if (string.IsNullOrEmpty(getOptions.ConsulHost) == false)
                    consulAddress = new Uri(getOptions.ConsulHost);
                var consul = new ConsulForPandora(consulAddress);
                ApplicationContext currentContext = new ApplicationContext(applicationName, cluster, machine);

                var pandora = new Elders.Pandora.Pandora(currentContext, consul);
                var value = pandora.Get(getOptions.Key);
                Console.WriteLine(value);
            }

            return string.Empty;
        }

        private static string OpenComand(object invokedVerbInstance)
        {
            var openOptions = (OpenOptions)invokedVerbInstance;

            string applicationName = openOptions.Application;
            string cluster = openOptions.Cluster;
            string machine = openOptions.Machine;
            string jarFile = openOptions.Jar ?? openOptions.Application + ".json";
            if (!File.Exists(jarFile)) throw new FileNotFoundException("Jar file is required.", jarFile);

            var jar = JsonConvert.DeserializeObject<Elders.Pandora.Box.Jar>(File.ReadAllText(jarFile));
            var box = Box.Mistranslate(jar);
            if (box.Name.Equals(applicationName, StringComparison.OrdinalIgnoreCase) == false) throw new InvalidProgramException("Invalid application name");

            var cfg = box.Open(new PandoraOptions(cluster, machine));

            if (openOptions.Output == OpenOptions.EnvVarOutput)
            {
                foreach (var setting in cfg.AsDictionary())
                {
                    Environment.SetEnvironmentVariable(setting.Key, ConvertToString(setting.Value), EnvironmentVariableTarget.Machine);
                }
                return "Wrote to EnvVar!";
            }
            else if (openOptions.Output == OpenOptions.ConsulOutput)
            {
                Uri consulAddress = null;
                if (string.IsNullOrEmpty(openOptions.ConsulHost) == false)
                    consulAddress = new Uri(openOptions.ConsulHost);
                var consul = new ConsulForPandora(consulAddress);
                var currentContext = new ApplicationContext(applicationName, cluster, machine);

                var pandora = new Elders.Pandora.Pandora(currentContext, consul);

                foreach (var setting in pandora.GetAll(currentContext))
                {
                    pandora.Delete(setting.Key.SettingKey);
                }

                foreach (var setting in cfg.AsDictionary())
                {
                    consul.Set(setting.Key, ConvertToString(setting.Value));
                }

                return "Wrote to Consul!";
            }
            else
            {
                var computedCfg = JsonConvert.SerializeObject(cfg.AsDictionary());
                File.WriteAllText(openOptions.Output, computedCfg);
                return "Wrote to textFile";
            }
        }

        private static string ConvertToString(object value)
        {
            var stringValue = string.Empty;

            var converter = TypeDescriptor.GetConverter(typeof(string));
            if (converter.IsValid(value))
            {
                stringValue = (string)converter.ConvertFrom(value);
            }
            else
            {
                stringValue = JsonConvert.SerializeObject(value);
            }

            return stringValue;
        }
    }
}
