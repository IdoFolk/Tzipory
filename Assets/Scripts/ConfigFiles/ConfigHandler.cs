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
            
            return  default;
        }
    }
}