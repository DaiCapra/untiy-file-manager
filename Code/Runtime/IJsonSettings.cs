using Newtonsoft.Json;

namespace FileManagement.Code.Runtime
{
    public interface IJsonSettings
    {
        JsonSerializerSettings Settings { get; }
    }
}