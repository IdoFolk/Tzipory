using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.ConfigFiles.Item;
using Tzipory.Helpers.Consts;
using UnityEngine;


namespace Tzipory.ConfigFiles
{
    [System.Serializable]
    public class ConfigManager
    {
        [SerializeField] private ConfigHandler<ShamanConfig> _shamanConfig;
        [SerializeField] private ConfigHandler<EnemyConfig> _enemyConfig;
        [SerializeField] private ConfigHandler<AbilityConfig> _abilityConfig;
        [SerializeField] private ConfigHandler<ItemConfig> _itemConfig;
        //add item config handler
        
        
        public IConfigFile GetConfig(int dataId,int objectId)
        {
            return dataId switch
            {
                Constant.DataId.SHAMAN_DATA_ID => _shamanConfig.GetConfigFile(objectId),
                Constant.DataId.ENEMY_DATA_ID => _enemyConfig.GetConfigFile(objectId),
                Constant.DataId.ABILITY_DATA_ID => _abilityConfig.GetConfigFile(objectId),
                Constant.DataId.ITEM_DATA_ID => _itemConfig.GetConfigFile(objectId),
                _ => null
            };
        }
    }
}