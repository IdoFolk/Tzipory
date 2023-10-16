using DefaultNamespace;
using Tzipory.ConfigFiles.Item;
using Tzipory.Helpers.Consts;
using UnityEngine;

public class CraftingSystemController : MonoBehaviour
{
    [SerializeField] CraftingSystemConfig config;

    public ItemConfig CompareMaterialsAndGetItem(CurrencyContainer[] _userCurrencies)
    {
        foreach (var recipe in config.Recipes)
        {
            bool isRecipeViable = true;

            for (int i = 0; i < recipe.Currencies.Length; i++)
            {
                bool currencyFound = false;
                foreach (var userCurrency in _userCurrencies)
                {
                    if (userCurrency.Material == recipe.Currencies[i].Material && userCurrency.Amount >= recipe.Currencies[i].Amount)
                    {
                        currencyFound = true;
                        break;//break foreach
                        //same currency found with sufficient amount
                    }
                   
                }
                if(!currencyFound)
                {
                    isRecipeViable = false;
                    break;//break for
                }
               
            }
            if (isRecipeViable)
            {
                return recipe.ResultingItem;
            }

        }
        return null;
    }


    /// <summary>
    /// Debugging from here can be deleted later
    /// </summary>
    [ContextMenu("TestGettingitemConfigResultNull")]
    public void TestGettingitemConfigResultNull()
    {
        CurrencyContainer[] UserTestCurrencies = new CurrencyContainer[2]//wrong amount
        {
            new CurrencyContainer(Constant.Materials.Bones,2),
            new CurrencyContainer(Constant.Materials.Honey,3)
        };
        ItemConfig resultingItem = CompareMaterialsAndGetItem(UserTestCurrencies);
        if (resultingItem == null)
        {
            Debug.Log("No Item Found");
        }
        else
        {
            Debug.Log($"ItemFound{resultingItem.name}");
        }
    }
    [ContextMenu("TestGettingitemConfigResultItem")]
    public void TestGettingitemConfigResultItem()
    {
        CurrencyContainer[] UserTestCurrencies = new CurrencyContainer[2]//correct amount
        {
            new CurrencyContainer(Constant.Materials.Honey,4),
            new CurrencyContainer(Constant.Materials.Bones,2)
        };
        ItemConfig resultingItem = CompareMaterialsAndGetItem(UserTestCurrencies);
        if (resultingItem == null)
        {
            Debug.Log("No Item Found");
        }
        else
        {
            Debug.Log($"ItemFound{resultingItem.name}");
        }
    }
}
