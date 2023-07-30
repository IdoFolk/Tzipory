using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.EntitySystem.EntityConfigSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles
{
    [System.Serializable]
    public class ConfigManager
    {
        [SerializeField] private ConfigHandler<ShamanConfig> _shamanConfig;
        [SerializeField] private ConfigHandler<EnemyConfig> _enemyConfig;
        [SerializeField] private ConfigHandler<AbilityConfig> _abilityConfig;
        //add item config handler
        
        public IConfigFile GetConfig(int dataId,int objectId)
        {
            return null;
        }
    }
}