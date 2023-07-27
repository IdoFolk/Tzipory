using Helpers.Consts;
using UnityEngine;

namespace Tzipory.EntitySystem.EntityConfigSystem
{
    [CreateAssetMenu(fileName = "New shaman config", menuName = "ScriptableObjects/Entity/New shaman config", order = 0)]
    public class ShamanConfig : BaseUnitEntityConfig
    {
        [SerializeField] private int _shamanId;
        
        public override int ConfigObjectId => _shamanId;
        public override int ConfigTypeId => Constant.DataId.ShamanDataID;
    }
}