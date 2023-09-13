using System.Collections.Generic;
using Systems.SaveLoadSystem;
using Tzipory.ConfigFiles.PartyConfig;
using Tzipory.ConfigFiles.WaveSystemConfig;
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
            
            if (_saveAndLoadManager.GetSaveData(out T data))
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
            
            if (_saveAndLoadManager.GetSaveData(out T data))
            {
                //return save data
            }
            
            var configFile = _configManager.GetConfig(output.SerializeTypeId,objectId);
            
            if (!output.IsInitialization)
                output.Init(configFile);
            
            return output;
        }

        public IEnumerable<T> GetDatas<T>(IConfigFile configFile) where T : class, ISerializeData, new()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetDatas<T>(int objectId) where T : class, ISerializeData, new()
        {
            var output = new List<T>();
            
            if (_saveAndLoadManager.GetSaveData(out IEnumerable<T> data))
            {
                output.AddRange(data);
                //return save data
            }

            foreach (var serializeData in output)
            {
                var configFile = _configManager.GetConfig(serializeData.SerializeTypeId,objectId);
            
                if (!serializeData.IsInitialization)
                    serializeData.Init(configFile);
            }
            
            return output;
        }

        private void SaveData()
        {
            //SaveAllData
        }
    }
}