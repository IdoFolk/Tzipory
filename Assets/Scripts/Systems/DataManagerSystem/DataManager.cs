using GameplayeLogic.Managersp;
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
        
        private PlayerSerializeData _playerData;
        //PlayerSerializeData
        //MapData
        //GameData

        public static IDataRequester DataRequester { get; private set; }
        
        private void Awake()
        {
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
            
            var config = _configManager.GetConfig(output.SerializeTypeId,objectId);
            
            output.Init(config);
            
            return output;
        }

        private void SaveData()
        {
            //SaveAllData
        }
    }
}