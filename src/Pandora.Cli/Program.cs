using CommandLine;
using Elders.Pandora;
using Elders.Pandora.Box;
using Newtonsoft.Json;
using Pandora.Cli.Core.OptionTypes;
using System;
using System.Collections.Generic;
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
                Parser.Default
                    .ParseArguments<OpenOptions, GetOptions, ValidateOptions>(args)
                    .MapResult(
                        (OpenOptions opts) => OpenCommand(opts),
                        (GetOptions opts) => GetCommand(opts),
                        (ValidateOptions opts) => ValidateCommand(opts),
                        _ => "Could not set the variable!"
                    );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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

        private static string OpenCommand(object invokedVerbInstance)
        {
            var openOptions = (OpenOptions)invokedVerbInstance;

            string applicationName = openOptions.Application;
            string cluster = openOptions.Cluster;
            string machine = openOptions.Machine;
            string jarFile = openOptions.Jar ?? openOptions.Application + ".json";
            if (string.IsNullOrEmpty(openOptions.WorkingDirectory) == false)
                Directory.SetCurrentDirectory(openOptions.WorkingDirectory);

            if (!File.Exists(jarFile)) throw new FileNotFoundException("Jar file is required.", jarFile);

            Jar jar = JsonConvert.DeserializeObject<Jar>(File.ReadAllText(jarFile));
            Box box = Box.Mistranslate(jar);
            if (box.Name.Equals(applicationName, StringComparison.OrdinalIgnoreCase) == false) throw new InvalidProgramException("Invalid application name");

            Configuration cfg = box.Open(new PandoraOptions(cluster, machine));

            if (openOptions.Output == OpenOptions.EnvVarOutput)
            {

                foreach (KeyValuePair<string, object> setting in cfg.AsDictionary())
                {
                    if (cfg.IsDynamic(setting.Key)) // If the setting is dynamic and it already exist in Env Vars, we must skip it
                    {
                        string envVar = Environment.GetEnvironmentVariable(setting.Key);
                        if (string.IsNullOrEmpty(envVar) == false) continue;
                    }

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

                foreach (DeployedSetting setting in pandora.GetAll(currentContext))
                {
                    // We must skip all dynamic settings
                    string key = NameBuilder.GetSettingName(setting.Key.ApplicationName, setting.Key.Cluster, setting.Key.Machine, setting.Key.SettingKey);
                    if (cfg.IsDynamic(key))
                        continue;

                    pandora.Delete(setting.Key.SettingKey);
                }

                foreach (KeyValuePair<string, object> setting in cfg.AsDictionary())
                {
                    if (cfg.IsDynamic(setting.Key)) // If the setting is dynamic and it already exist in consul, we must skip it
                    {
                        bool exist = consul.Exists(setting.Key);
                        if (exist) continue;
                    }

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
