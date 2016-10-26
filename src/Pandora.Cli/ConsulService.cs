using System.Text;
using System.Threading.Tasks;
using Consul;
using System;
using System.Collections.Generic;

namespace Elders.Pandora.Cli
{
    class ConsulService
    {
        public ConsulService()
        {
            var consulClientConfig = new ConsulClientConfiguration();
            Config = consulClientConfig;
        }

        private ConsulClientConfiguration Config { get; set; }

        public void setAddress(Uri address)
        {
            Config.Address = address;
        }

        public async Task<bool> KvPut(string key, string value)
        {
            try
            {
                using (var client = new ConsulClient((conf) => conf = this.Config))
                {
                    var putPair = new KVPair(key)
                    {
                        Value = Encoding.UTF8.GetBytes(value)
                    };

                    var putAttempt = await client.KV.Put(putPair);

                    if (putAttempt.Response)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<bool> KvPutDictionary(Dictionary<string, string> dict)
        {
            try
            {
                using (var client = new ConsulClient((conf) => conf = this.Config))
                {
                    foreach (var item in dict)
                    {
                        var putPair = new KVPair(item.Key)
                        {
                            Value = Encoding.UTF8.GetBytes(item.Value)
                        };

                        var putAttempt = await client.KV.Put(putPair);

                        if (!putAttempt.Response)
                            return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
    }
}
