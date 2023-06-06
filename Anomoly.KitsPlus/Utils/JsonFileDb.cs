using Newtonsoft.Json;
using Rocket.Core.Logging;
using System;
using System.IO;

namespace Anomoly.KitsPlus.Utils
{
    public class JsonFileDb<T> where T: class
    {
        private string _filePath;
        private T _defaultInstance;
        private Formatting _jsonFormatting;

        public T Instance { get; private set; }
        public JsonFileDb(string filePath, T defaultInstance = null, Formatting jsonFormatting = Formatting.None)
        {
            _filePath = filePath;
            _defaultInstance = defaultInstance;
            _jsonFormatting = jsonFormatting;
        }

        public void Save()
        {
            if (Instance == null)
                Instance = _defaultInstance;
            
            string json = null;
            try
            {
                json = JsonConvert.SerializeObject(Instance, _jsonFormatting);
            }
            catch(Exception ex) { Logger.LogException(ex, $"Failed to serialize json object for saving: {_filePath}"); }
            
            File.WriteAllText(_filePath, json);
        }

        public void Load()
        {
            if (!File.Exists(_filePath))
            {
                Instance = _defaultInstance;
                Save();
                return;
            }

            var json = File.ReadAllText(_filePath);

            try
            {
                Instance = JsonConvert.DeserializeObject<T>(json);
            }
            catch(Exception ex) { Logger.LogException(ex, $"Failed to parse json file: {_filePath}"); }
        }
    }
}
