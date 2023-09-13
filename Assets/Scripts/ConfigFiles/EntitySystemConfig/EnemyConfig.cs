using Tzipory.GameplayLogic.StatusEffectTypes;
using Helpers.Consts;
using Sirenix.OdinInspector;
using UnityEngine;
using Tzipory.ConfigFiles.PartyConfig.EntitySystemConfig;

namespace Tzipory.ConfigFiles.PartyConfig.EntitySystemConfig
{
    [CreateAssetMenu(fileName = "New enemy config", menuName = "ScriptableObjects/Entity/New enemy config", order = 0)]
    public class EnemyConfig : BaseUnitEntityConfig
    {
        [SerializeField] private int _enemyId;
        [SerializeField,TabGroup("AI")] private float _decisionInterval;//temp
        [SerializeField,TabGroup("AI")] private float _aggroLevel;//temp
        [SerializeField,TabGroup("AI")] private float _returnLevel;//temp

        public float DecisionInterval => _decisionInterval;

        public float AggroLevel => _aggroLevel;

        public float ReturnLevel => _returnLevel;

        public override int ConfigObjectId => _enemyId;
        public override int ConfigTypeId => Constant.DataId.ENEMY_DATA_ID;

        protected override void OnValidate()
        {
            base.OnValidate();
            if (_statConfigs.Count == 0)
                _statConfigs.Add(new StatConfig() { _statsId = Constant.StatsId.CoreAttackDamage });
        }
    }
}