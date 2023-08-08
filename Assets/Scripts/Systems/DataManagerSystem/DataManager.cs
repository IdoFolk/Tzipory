using Systems.SaveLoadSystem;
using Tzipory.ConfigFiles;
using Tzipory.SerializeData;
using UnityEngine;

namespace Systems.DataManagerSystem
{
    public class DataManager : MonoBehaviour , IDataRequester
    {
        [SerializeField] private ConfigManager _configManager;
        private SaveAndLoadManager _saveAndLoadManager;
        
        public static IDataRequester DataRequester { get; private set; }
        
        public ConfigManager ConfigManager { get; private set;}
        
        private void Awake()
        {
            ConfigManager = _configManager;
            _saveAndLoadManager = new SaveAndLoadManager();
            
            if (DataRequester == null)
                DataRequester = this;
        }


        public T GetData<T>(IConfigFile configFile) where T : class, ISerializeData , new()
        {
            var output = new T();
            
            if (_saveAndLoadManager.GetSaveData())
            {
                //return save data
            }

            if (!output.IsInitialization)
                output.Init(configFile);
            
            return output;
        }

        public T GetData<T>(int objectId) where T : class, ISerializeData, new()
        {
            var output = new T();
            
            if (_saveAndLoadManager.GetSaveData())
            {
                //return save data
            }
            
            var configFile = _configManager.GetConfig(output.SerializeTypeId,objectId);
            
            if (!output.IsInitialization)
                output.Init(configFile);
            
            return output;
        }

        private void SaveData()
        {
            //SaveAllData
        }
    }
}