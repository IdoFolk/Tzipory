using Tzipory.SerializeData.CurrencySystem;
using UnityEngine;

[CreateAssetMenu(fileName = "New Recipe", menuName = "ScriptableObjects/Recipe", order = 1)]
public class RecipeConfig : ScriptableObject
{
    [SerializeField] private CurrencySerializeData[] _currencies;

    public CurrencySerializeData[] Currencies => _currencies;
}
