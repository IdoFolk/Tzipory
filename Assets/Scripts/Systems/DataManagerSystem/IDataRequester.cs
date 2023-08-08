using Tzipory.ConfigFiles;
using Tzipory.SerializeData;

namespace Systems.DataManagerSystem
{
    public interface IDataRequester
    {
        public ConfigManager ConfigManager { get;}//temp
        
        public T GetData<T>(IConfigFile configFile) where T : class, ISerializeData, new();
        public T GetData<T>(int objectId) where T : class, ISerializeData, new();
    }
}