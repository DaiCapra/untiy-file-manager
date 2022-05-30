using System;
using System.IO;
using Newtonsoft.Json;

namespace FileManagement.Code.Runtime
{
    public class FileManager : IFileManager
    {
        private readonly IJsonSettings _jsonSettings;
        private readonly string _nameTempFile;

        public FileManager(IJsonSettings jsonSettings)
        {
            _nameTempFile = "temp";
            _jsonSettings = jsonSettings;
        }

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, _jsonSettings.Settings);
        }

        public T Deserialize<T>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default;
            }

            var obj = JsonConvert.DeserializeObject<T>(json, _jsonSettings.Settings);
            return obj;
        }

        public void Save<T>(string filepath, T data, string backupFilepath = null, bool validateSaveFile = true)
        {
            if (filepath == null)
            {
                throw new NullReferenceException("Filepath can't be nulL!");
            }

            if (data == null)
            {
                throw new NullReferenceException("Data can't be nulL!");
            }

            // ensure that target directory exists
            var directory = EnsureDirectory(filepath);

            // filepath for temp file
            var filepathWithoutExtension = Path.GetFileNameWithoutExtension(filepath);
            var extension = Path.GetExtension(filepath);

            var fileTemp = $"{filepathWithoutExtension}-{_nameTempFile}{extension}";
            var filepathTemp = Path.Combine(directory, fileTemp);
            
            // save to temp file
            var json = Serialize(data);
            File.WriteAllText(filepathTemp, json);

            // validate temp file
            if (validateSaveFile)
            {
                var fileJson = File.ReadAllText(filepathTemp);
                var validation = Deserialize<T>(fileJson);
                if (validation == null)
                {
                    throw new FileLoadException("Couldn't validate save file!");
                }
            }

            // delete previous file at target filepath
            if (File.Exists(filepath))
            {
                // make backup if a path was provided
                if (!string.IsNullOrEmpty(backupFilepath))
                {
                    File.Copy(filepath, backupFilepath);
                }

                File.Delete(filepath);
            }

            // move temp file to target filepath
            File.Move(filepathTemp, filepath);
        }

        private string EnsureDirectory(string filepath)
        {
            var directory = Path.GetDirectoryName(filepath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            return directory;
        }

        public T Load<T>(string filepath)
        {
            if (filepath == null)
            {
                throw new NullReferenceException("Filepath can't be nulL!");
            }

            if (File.Exists(filepath))
            {
                var json = File.ReadAllText(filepath);
                var data = Deserialize<T>(json);
                return data;
            }

            return default;
        }
    }
}