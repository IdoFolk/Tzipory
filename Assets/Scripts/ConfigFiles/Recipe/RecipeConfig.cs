using Tzipory.SerializeData.CurrencySystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.Recipe
{
    [CreateAssetMenu(fileName = "New Recipe", menuName = "ScriptableObjects/Recipe", order = 1)]
    public class RecipeConfig : ScriptableObject
    {
        [SerializeField] private CurrencySerializeData[] _currencies;

        public CurrencySerializeData[] Currencies => _currencies;
    }
}
