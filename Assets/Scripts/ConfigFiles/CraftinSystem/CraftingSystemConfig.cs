using System.Collections;
using System.Collections.Generic;
using Tzipory.ConfigFiles.Item;
using UnityEngine;
[CreateAssetMenu(fileName = "New Craft System Config", menuName = "ScriptableObjects/Crafting System Config", order = 2)]

public class CraftingSystemConfig : ScriptableObject
{
    [SerializeField] ItemConfig[] _items;

    public ItemConfig[] Items { get => _items; }
}
