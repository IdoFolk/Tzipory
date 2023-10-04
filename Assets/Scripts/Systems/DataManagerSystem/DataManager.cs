using System.Collections.Generic;
using Tzipory.ConfigFiles;
using Tzipory.Systems.SaveLoadSystem;
using Tzipory.ConfigFiles.Party;
using Tzipory.SerializeData;
using UnityEngine;

namespace Tzipory.Systems.DataManager
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


        public T GetSerializeData<T>(IConfigFile configFile) where T : class, ISerializeData , new()
        {
            var output = new T();
            
            if (_saveAndLoadManager.GetSaveData(out T serializeData))
            {
                //return save data
                return serializeData;
            }

            if (!output.IsInitialization)
                output.Init(configFile);
            
            return output;
        }

        public T GetSerializeData<T>(int objectId) where T : class, ISerializeData, new()
        {
            var output = new T();
            
            if (_saveAndLoadManager.GetSaveData(out T serializeData))
            {
                //return save data
                return serializeData;
            }
            
            var configFile = _configManager.GetConfig(output.SerializeTypeId,objectId);
            
            if (!output.IsInitialization)
                output.Init(configFile);
            
            return output;
        }

        public T GetConfigData<T>(int objectId) where T : class, IConfigFile, new()
        {
            var output = new T();
            
            var configFile = _configManager.GetConfig(output.ConfigTypeId,objectId);
            
            return configFile as T;
        }

        public IEnumerable<T> GetSerializeDatas<T>(IConfigFile configFile) where T : class, ISerializeData, new()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<T> GetSerializeDatas<T>(int objectId) where T : class, ISerializeData, new()
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