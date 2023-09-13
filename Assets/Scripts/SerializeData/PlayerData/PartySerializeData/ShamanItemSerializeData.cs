using Helpers.Consts;
using Tzipory.ConfigFiles.PartyConfig;

namespace Tzipory.ConfigFiles.WaveSystemConfig
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

        public int ItemInstanceId => _itemInstanceId;
        public ItemSlot TargetSlot => targetSlot;

//TODO return to private, temp for testing
        public int _itemId;
        public int _itemInstanceId;
        private ItemSlot targetSlot;
    }
}