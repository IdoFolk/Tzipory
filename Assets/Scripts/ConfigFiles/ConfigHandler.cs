using Helpers.Consts;
using UnityEngine;

namespace Tzipory.ConfigFiles
{
    [System.Serializable]
    public class ConfigHandler<T> where T : IConfigFile
    {
        [SerializeField] private T[] _config;

        public int ConfigTypeId => Constant.DataId.ShamanDataID;

        public T GetConfigFile(int id)
        {
            foreach (var configFile in _config) 
            {
                if (configFile.ConfigObjectId == id)
                    return configFile;
            }
            
            return  default;
        }
    }
}