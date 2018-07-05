using Elders.Pandora.Cli.Logging;
using Newtonsoft.Json;
using System.IO;

namespace Elders.Pandora.Cli.Validator
{
    public static class Validator
    {
        static ILog log = LogProvider.GetLogger(typeof(Validator));

        public static void Validate(string path, string fileName)
        {
            log.Info($"STARTED -> validating '{fileName}' in directory '{path}'");
            var mainJar = ReadJarFromFile($"{path}/{fileName}");
            if (mainJar.IsValid() == false)
            {
                log.Error($"FAILED -> validating '{fileName}' in directory '{path}'");
                return;
            }
            else
            {
                log.Info($"JAR {mainJar.Name} was validated.");
            }

            log.Info($"STARTED -> validating references of '{fileName}' in directory '{path}'");


            if (ReferenceEquals(null, mainJar.References) == false)
            {
                foreach (var reference in mainJar.References)
                {
                    Validate(path, reference.Jar.ToString());
                }
                log.Info($"FINISHED -> validating references of '{fileName}' in directory '{path}'");
            }

            log.Info($"FINISHED -> validating '{fileName}' in directory '{path}'");
        }

        static Jar ReadJarFromFile(string filePath)
        {
            try
            {
                var json = File.ReadAllText(filePath);

                Jar result = JsonConvert.DeserializeObject<Jar>(json);

                return result;
            }
            catch (FileNotFoundException ex)
            {
                throw new ValidatorException($"Jar was not found : {filePath}", ex);
            }
            catch (JsonReaderException ex)
            {
                throw new ValidatorException($"Jar on {filePath} is not a valid JSON.", ex);
            }
        }
    }
}
