using Helpers.Consts;
using Tzipory.ConfigFiles;

namespace Tzipory.SerializeData
{
    public enum ItemSlot
    {
        Head,
        Neck,
        Chest,
        Legs,
        Hands
    }
    
    [System.Serializable]
    public class ShamanItemSerializeData: ISerializeData
    {
        public bool IsInitialization { get; private set; }
        public void Init(IConfigFile parameter)
        {
            IsInitialization = true;
        }

        public int SerializeTypeId => Constant.DataId.SHAMAN_ITEM_DATA_ID;
        
        public int ItemId => _itemId;
        public ItemSlot TargetSlot => targetSlot;

        private int _itemId;
        private ItemSlot targetSlot;
    }
}