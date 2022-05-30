namespace FileManagement.Code.Runtime
{
    public interface IFileManager
    {
        T Deserialize<T>(string json);
        string Serialize<T>(T obj);
    }
}