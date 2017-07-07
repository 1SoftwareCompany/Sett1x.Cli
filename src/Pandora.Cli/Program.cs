using System;
using System.IO;
using Elders.Pandora.Box;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Elders.Pandora.Cli
{
    public class Program
    {
        public static int Main(string[] args)
        {
            try
            {
                string invokedVerb = string.Empty;
                object invokedVerbInstance = null;

                var options = new Options();

                if (args == null || args.Length == 0)
                {
                    Console.WriteLine(options.GetUsage());
                    return 0;
                }

                if (!CommandLine.Parser.Default.ParseArguments(args, options, (verb, subOptions) => { invokedVerb = verb; invokedVerbInstance = subOptions; }))
                {
                    Console.WriteLine(options.GetUsage());
                    return 0;
                }

                if (invokedVerb.Equals("open", StringComparison.OrdinalIgnoreCase))
                {
                    var openOptions = (OpenOptions)invokedVerbInstance;

                    string applicationName = openOptions.Application;
                    string cluster = openOptions.Cluster;
                    string machine = openOptions.Machine;
                    string jarFile = openOptions.Jar ?? openOptions.Application + ".json";
                    if (!File.Exists(jarFile)) throw new FileNotFoundException("Jar file is required.", jarFile);

                    var jar = JsonConvert.DeserializeObject<Jar>(File.ReadAllText(jarFile));
                    var box = Box.Box.Mistranslate(jar);
                    if (box.Name.Equals(applicationName, StringComparison.OrdinalIgnoreCase) == false) throw new InvalidProgramException("Invalid application name");

                    var cfg = box.Open(new PandoraOptions(cluster, machine));

                    if (openOptions.Output == OpenOptions.EnvVarOutput)
                    {
                        foreach (var setting in cfg.AsDictionary())
                        {
                            Environment.SetEnvironmentVariable(setting.Key, ConvertToString(setting.Value), EnvironmentVariableTarget.Machine);
                        }
                    }
                    else if (openOptions.Output == OpenOptions.ConsulOutput)
                    {
                        Uri consulAddress = null;
                        if (string.IsNullOrEmpty(openOptions.ConsulHost) == false)
                            consulAddress = new Uri(openOptions.ConsulHost);
                        var consul = new ConsulForPandora(consulAddress);
                        ApplicationContext currentContext = (ApplicationContext)ApplicationConfiguration.CreateContext(applicationName, cluster, machine);

                        var pandora = new Pandora(currentContext, consul);

                        foreach (var setting in pandora.GetAll(currentContext))
                        {
                            pandora.Delete(setting.Key.SettingKey);
                        }

                        foreach (var setting in cfg.AsDictionary())
                        {
                            consul.Set(setting.Key, ConvertToString(setting.Value));
                        }
                    }
                    else
                    {
                        var computedCfg = JsonConvert.SerializeObject(cfg.AsDictionary());
                        File.WriteAllText(openOptions.Output, computedCfg);
                    }
                }
                else if (invokedVerb.Equals("get", StringComparison.OrdinalIgnoreCase))
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
                        ApplicationContext currentContext = (ApplicationContext)ApplicationConfiguration.CreateContext(applicationName, cluster, machine);

                        var pandora = new Pandora(currentContext, consul);
                        var value = pandora.Get(getOptions.Key);
                        Console.WriteLine(value);
                    }
                }
            }
            catch (Exception)
            {
                return 1;
            }

            return 0;
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
