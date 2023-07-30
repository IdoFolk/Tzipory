using Helpers.Consts;
using Systems.SaveLoadSystem;
using Tzipory.ConfigFiles;
using Tzipory.SerializeData;
using Tzipory.SerializeData.AbilitySystemSerializeData;
using UnityEngine;

namespace Systems.DataManagerSystem
{
    public class DataManager : MonoBehaviour
    {
        [SerializeField] private ConfigManager _configManager;
        private SaveAndLoadManager _saveAndLoadManager;

        private void Awake()
        {
            _saveAndLoadManager = new SaveAndLoadManager();
            GetData<AbilitySerializeData>(Constant.ShamanId.TOOR_ID);
        }

        public T GetData<T>(int objectId) where T : class, ISerializeData , new()
        {
            T output = new T();
            
            if (_saveAndLoadManager.GetSaveData())
            {
                //return save data
            }
            
            var config = _configManager.GetConfig(output.SerializeTypeId,objectId);
            
            output.Init(config);
            
            return output;
        }
        
    }
}