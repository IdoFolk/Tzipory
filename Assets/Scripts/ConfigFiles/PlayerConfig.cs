using DefaultNamespace;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.Inventory;
using Tzipory.Helpers.Consts;
using UnityEngine;

namespace Tzipory.ConfigFiles.Party
{
    [CreateAssetMenu(fileName = "NewPlayerConfig", menuName = "ScriptableObjects/Config/Player config", order = 0)]
    public class PlayerConfig : ScriptableObject , IConfigFile
    {
        public int ObjectId { get; }
        
        public int ConfigTypeId => Constant.DataId.PLAYER_DATA_ID;

        [SerializeField,TabGroup("Party Config")] private PartyConfig _partyConfig;
        [SerializeField,TabGroup("Inventory Config")] private InventoryConfig _inventoryConfig;
        [SerializeField,TabGroup("Currency Config")] private PlayerCurrencyConfig _currencyConfig;


        public InventoryConfig InventoryConfig => _inventoryConfig;
        public PartyConfig PartyConfig => _partyConfig;

        public PlayerCurrencyConfig CurrencyConfig => _currencyConfig;
    }
}