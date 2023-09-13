using System.Collections.Generic;
using Tzipory.ConfigFiles.PartyConfig;
using Tzipory.ConfigFiles.WaveSystemConfig;

namespace Systems.DataManagerSystem
{
    public interface IDataRequester
    {
        public ConfigManager ConfigManager { get;}//temp
        
        public T GetData<T>(IConfigFile configFile) where T : class, ISerializeData, new();
        public T GetData<T>(int objectId) where T : class, ISerializeData, new();
        
        public IEnumerable<T> GetDatas<T>(IConfigFile configFile) where T : class, ISerializeData, new();
        public IEnumerable<T> GetDatas<T>(int objectId) where T : class, ISerializeData, new();
    }
}