using Tzipory.ConfigFiles.Item;
using UnityEngine;

namespace Tzipory.ConfigFiles.Crafting
{
    [CreateAssetMenu(fileName = "New Craft System Config", menuName = "ScriptableObjects/Crafting System Config", order = 2)]

    public class CraftingSystemConfig : ScriptableObject
    {
        [SerializeField] private ItemConfig[] _items;

        public ItemConfig[] Items => _items;
    }
}
