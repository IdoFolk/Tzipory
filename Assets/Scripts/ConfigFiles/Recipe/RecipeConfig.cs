using DefaultNamespace;
using System;
using System.Collections;
using System.Collections.Generic;
using Tzipory.ConfigFiles.Item;
using UnityEngine;
[CreateAssetMenu(fileName = "New Recipe", menuName = "ScriptableObjects/Recipe", order = 1)]
public class RecipeConfig : ScriptableObject
{
    [SerializeField] CurrencyContainer[] _currencies;
    [SerializeField] ItemConfig _resultingItem;

    public CurrencyContainer[] Currencies { get => _currencies; }
    public ItemConfig ResultingItem { get => _resultingItem;}
}
