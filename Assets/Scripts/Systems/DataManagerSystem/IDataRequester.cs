using System.Collections.Generic;
using Tzipory.ConfigFiles;
using Tzipory.SerializeData;

namespace Tzipory.Systems.DataManager
{
    public interface IDataRequester
    {
        public ConfigManager ConfigManager { get;}//temp
        
        public T GetSerializeData<T>(IConfigFile configFile) where T : class, ISerializeData, new();
        public T GetSerializeData<T>(int objectId) where T : class, ISerializeData, new();
        public T GetConfigData<T>(int objectId) where T : class, IConfigFile, new();
        public IEnumerable<T> GetSerializeDatas<T>(IConfigFile configFile) where T : class, ISerializeData, new();
        public IEnumerable<T> GetSerializeDatas<T>(int objectId) where T : class, ISerializeData, new();
    }
}