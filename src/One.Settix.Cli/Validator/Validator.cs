using Newtonsoft.Json;
using System.IO;

namespace One.Settix.Cli.Core.Validator
{
    public static class Validator
    {
        public static void Validate(string path, string fileName)
        {
            var mainJar = ReadJarFromFile($"{path}/{fileName}");
            if (mainJar.IsValid() == false)
            {
                throw new ValidatorException($"FAILED -> validating '{path}/{fileName}' in directory '{path}'");
            }

            if (mainJar.References is null == false)
            {
                foreach (var reference in mainJar.References)
                {
                    Validate(path, reference.Jar.ToString());
                }
            }
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
