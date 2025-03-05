using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Pandora.Cli.Core.Validator
{
    public class Jar
    {
        public string Name { get; set; }

        public IEnumerable<Reference> References { get; set; }

        public object Defaults { get; set; }

        public object Clusters { get; set; }

        public object Machines { get; set; }

        public object DynamicSettings { get; set; }

        public bool IsValid()
        {
            return AreFieldNamesValid();
        }

        private bool AreFieldNamesValid()
        {
            var defaults = GetDefaults();

            if ((JObject)Clusters is null)
                return true;

            foreach (var prop in (JObject)Clusters)
            {
                var clusterName = prop.Key;

                foreach (var pr in (JObject)prop.Value)
                {
                    if (defaults.Contains(pr.Key) == false)
                        return false;
                }
            }

            if ((JArray)DynamicSettings is null)
                return true;

            foreach (JToken prop in (JArray)DynamicSettings)
            {
                if (defaults.Contains(prop.Value<string>()) == false)
                    return false;
            }

            return true;
        }

        private List<string> GetDefaults()
        {
            var defaults = new List<string>();

            foreach (var property in (JObject)Defaults)
            {
                var name = property.Key;

                defaults.Add(name);
            }

            return defaults;
        }
    }
}
