using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace FileManagement.Code.Runtime
{
    public class JsonSettings : IJsonSettings
    {
        public JsonSerializerSettings Settings { get; set; }

        public JsonSettings()
        {
            Settings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            };
        }
    }
}