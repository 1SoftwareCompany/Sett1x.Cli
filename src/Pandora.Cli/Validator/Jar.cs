using Elders.Pandora.Cli.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Elders.Pandora.Cli.Validator
{
    public class Jar
    {
        static ILog log = LogProvider.GetLogger(typeof(Jar));

        public string Name { get; set; }

        public IEnumerable<Reference> References { get; set; }

        public object Defaults { get; set; }

        public object Clusters { get; set; }

        public object Machines { get; set; }


        public bool IsValid()
        {
            return AreFieldNamesValid();
        }

        private bool AreFieldNamesValid()
        {
            var defaults = GetDefaults();

            if (ReferenceEquals(null, (JObject)Clusters))
                return true;

            foreach (var prop in (JObject)Clusters)
            {
                var clusterName = prop.Key;

                foreach (var pr in (JObject)prop.Value)
                {
                    if (defaults.Contains(pr.Key) == false)
                    {
                        log.Error($"Property '{pr.Key}' in cluster '{clusterName}' has wrong name ");

                        return false;
                    }
                }
            }


            return true;
        }

        private List<string> GetDefaults()
        {
            var defaults = new List<string>();

            dynamic result = JsonConvert.DeserializeObject<dynamic>(Defaults.ToString());

            foreach (var property in (JObject)Defaults)
            {
                var name = property.Key;

                defaults.Add(name);
            }

            return defaults;
        }
    }
}
