using System;
using UnityEngine;

namespace Tzipory.ConfigFiles
{
    [System.Serializable]
    public class ConfigHandler<T>  where T : IConfigFile
    {
        [SerializeField] private T[] _config;
        
        public T GetConfigFile(int id)
        {
            foreach (var configFile in _config) 
            {
                if (configFile.ObjectId == id)
                    return configFile;
            }

            throw new Exception($"Can not find {typeof(T).Name} with id {id} in {nameof(ConfigHandler<T>)}.{nameof(GetConfigFile)}(). Enter the missing config file to the config manager in the PersistentScene");
        }
    }
}