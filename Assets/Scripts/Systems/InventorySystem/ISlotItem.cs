using UnityEngine;

namespace Tzipory.Systems.InventorySystem
{
    public interface ISlotItem
    {
        public Sprite ItemSlotSprite { get; }
        public string ItemSlotName { get; }
        public string ItemSlotDescription { get; }
        public int ItemId { get; }
        public int ItemAmount { get;}
    }
}