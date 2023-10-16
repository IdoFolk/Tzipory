using DefaultNamespace;
using Tzipory.ConfigFiles.Item;
using Tzipory.Helpers.Consts;
using UnityEngine;

public class CraftingSystemController : MonoBehaviour
{
    [SerializeField] CraftingSystemConfig config;
    /// <summary>
    /// use this method to get the recipe of an item and display the currencies needed
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public RecipeConfig GetRecipeBasedOnItem(ItemConfig item)//Get recipe based on an item
    {
        foreach (var recipe in config.Recipes)
        {
            if (recipe.ResultingItem == item)
                return recipe;
        }
        Debug.Log("No Recipe Results in this Item");
        return null;
    }
    /// <summary>
    /// use these to check if you can craft an item
    /// </summary>
    /// <param name="_userCurrencies"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool CanPlayerCraftItem(CurrencyContainer[] _userCurrencies, ItemConfig item)
    {
        var recipe = GetRecipeBasedOnItem(item);
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
            if (!currencyFound)
            {
                isRecipeViable = false;
                break;//break for
            }

        }
        return isRecipeViable;
       
    }
    public bool CanPlayerCraftItem(CurrencyContainer[] _userCurrencies, RecipeConfig _recipe)//overload
    {

        bool isRecipeViable = true;

        for (int i = 0; i < _recipe.Currencies.Length; i++)
        {
            bool currencyFound = false;
            foreach (var userCurrency in _userCurrencies)
            {
                if (userCurrency.Material == _recipe.Currencies[i].Material && userCurrency.Amount >= _recipe.Currencies[i].Amount)
                {
                    currencyFound = true;
                    break;//break foreach
                          //same currency found with sufficient amount
                }

            }
            if (!currencyFound)
            {
                isRecipeViable = false;
                break;//break for
            }

        }
        return isRecipeViable;

    }


    /// <summary>
    /// you can safely ignore this method unless you want to send currencies and check with all recipes which recipe it correlates and get its item 
    /// </summary>
    /// <param name="_userCurrencies"></param>
    /// <returns></returns>
    public ItemConfig CompareMaterialsAndGetItem(CurrencyContainer[] _userCurrencies)//compare currencies to all recipes // deprecated
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
