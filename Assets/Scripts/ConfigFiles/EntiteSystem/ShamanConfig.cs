using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.Item;
using Tzipory.Helpers.Consts;
using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem
{
    [CreateAssetMenu(fileName = "New shaman config", menuName = "ScriptableObjects/Entity/New shaman config", order = 0)]
    public class ShamanConfig : BaseUnitEntityConfig
    {
        [SerializeField] private int _shamanId;

        #region Items

        [SerializeField, TabGroup("Items"),InfoBox("Item slot type do not match",InfoMessageType.Warning,"_earring1Check")] private ItemConfig _earring1;
        private bool _earring1Check => _earring1 is not null && _earring1 is not {ItemSlot: ItemSlot.Earring};
        
        [SerializeField, TabGroup("Items"),InfoBox("Item slot type do not match",InfoMessageType.Warning,"_earring2Check")] private ItemConfig _earring2;
        private bool _earring2Check => _earring2 is not null && _earring2 is not {ItemSlot: ItemSlot.Earring};
        
        [SerializeField, TabGroup("Items"),InfoBox("Item slot type do not match",InfoMessageType.Warning,"_necklaceCheck")] private ItemConfig _necklace;
        private bool _necklaceCheck => _necklace is not null && _necklace is not {ItemSlot: ItemSlot.Necklace};
        
        [SerializeField, TabGroup("Items"),InfoBox("Item slot type do not match",InfoMessageType.Warning,"_beltCheck")] private ItemConfig _belt;
        private bool _beltCheck => _belt is not null && _belt is not {ItemSlot: ItemSlot.Belt};
        
        [SerializeField, TabGroup("Items"),InfoBox("Item slot type do not match",InfoMessageType.Warning,"_bracelet1Check")] private ItemConfig _bracelet1;
        private bool _bracelet1Check => _bracelet1 is not null && _bracelet1 is not {ItemSlot: ItemSlot.Bracelet};
        
        [SerializeField, TabGroup("Items"),InfoBox("Item slot type do not match",InfoMessageType.Warning,"_bracelet2Check")] private ItemConfig _bracelet2;
        private bool _bracelet2Check => _bracelet2 is not null && _bracelet2 is not {ItemSlot: ItemSlot.Bracelet};
        
        [SerializeField, TabGroup("Items"),InfoBox("Item slot type do not match",InfoMessageType.Warning,"_ring1Check")] private ItemConfig _ring1;
        private bool _ring1Check => _ring1 is not null && _ring1 is not { ItemSlot: ItemSlot.Ring };
        
        [SerializeField, TabGroup("Items"),InfoBox("Item slot type do not match",InfoMessageType.Warning,"_ring2Check")] private ItemConfig _ring2;
        private bool _ring2Check => _ring2 is not null && _ring2 is not {ItemSlot: ItemSlot.Ring};

        #endregion
        
        [SerializeField,TabGroup("AI")] private float _decisionInterval;//temp
        public float DecisionInterval => _decisionInterval;

        public override int ObjectId => _shamanId;
        public override int ConfigTypeId => Constant.DataId.SHAMAN_DATA_ID;

        protected override void OnValidate()
        {
            base.OnValidate();
            
        }
    }
}