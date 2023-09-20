using Helpers.Consts;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.EntitySystem.EntityConfigSystem
{
    [CreateAssetMenu(fileName = "New shaman config", menuName = "ScriptableObjects/Entity/New shaman config", order = 0)]
    public class ShamanConfig : BaseUnitEntityConfig
    {
        [SerializeField] private int _shamanId;
        
        [SerializeField,TabGroup("AI")] private float _decisionInterval;//temp

        public float DecisionInterval => _decisionInterval;

        public override int ObjectId => _shamanId;
        public override int ConfigTypeId => Constant.DataId.SHAMAN_DATA_ID;
    }
}