using System.Collections.Generic;
using System.IO;
using System.Linq;
using Tzipory.Systems.SaveLoadSystem.PlayerPref;
using UnityEngine;

namespace Tzipory.Systems.SaveLoadSystem.SaveSystemJson
{
    public static class GameSaveUtilityJson
    {
        public static void SaveObject<T>(string path, T data) where T : class , ISave
        {
            string dataAsString = JsonUtility.ToJson(data);
            File.WriteAllText($"{path}/{data.ObjectName}.json", dataAsString);
            Debug.Log($"The file have been saved successfully key: {path}");
        }

        public static bool LoadObject<T>(string path,out T saveData) where T : class , ISave
        {
            if (!File.Exists(path))
            {
                saveData = default;
                Debug.LogWarning($"Can not find save data using key: {path}");
                return false;
            }
            
            string dataAsString = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<T>(dataAsString);
            Debug.Log($"The file have been loaded successfully key: {path}");
            return true;
        }
        
        public static void SaveObjects<T>(string path, IEnumerable<T> data) where T : class, ISave
        {
            if (!Directory.Exists(path))
            {
                Debug.LogWarning($"Can not find save data using key: {path}");
                Directory.CreateDirectory(path);
            }
            
            foreach (var save in data)
            {
                string dataAsString = JsonUtility.ToJson(save);
                File.WriteAllText($"{path}/{save.ObjectName}.json", dataAsString);
            }
            
            Debug.Log($"The files have been saved successfully key: {path}");
        }

        public static bool LoadObjects<T>(string path,out IEnumerable<T> saveData) where T : class ,ISave 
        {
            if (!Directory.Exists(path))
            {
                saveData = default;
                Debug.LogWarning($"Can not find save data using key: {path}");
                return false;
            }

            SerializeArray<T> serializeData = new SerializeArray<T>();
            var files = from file in Directory.EnumerateFiles(path,"*.json") select file;

            foreach (var file in files)
            {
                string dataAsString = File.ReadAllText(file);
                var fileData = JsonUtility.FromJson<T>(dataAsString);
                
                Debug.Log($"The file have been loaded successfully key: {file}");
                serializeData.SavedData.Add(fileData);
            }
            
            saveData = serializeData.SavedData;
            return true;
        }

        public static bool DeleteKey(string path)
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning($"Can not find and delete save data using key: {path}");
                return false;
            }
            
            File.Delete(path);
            Debug.Log($"The file have been deleted successfully key: {path}");
            return true;
        }
    }
}