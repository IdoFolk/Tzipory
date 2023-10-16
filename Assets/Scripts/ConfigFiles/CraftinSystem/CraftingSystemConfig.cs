using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Craft System Config", menuName = "ScriptableObjects/Crafting System Config", order = 2)]

public class CraftingSystemConfig : ScriptableObject
{
    [SerializeField] RecipeConfig[] _recipes;

    public RecipeConfig[] Recipes { get => _recipes;}
}
