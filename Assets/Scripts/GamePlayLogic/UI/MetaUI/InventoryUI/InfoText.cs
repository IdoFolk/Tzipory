using TMPro;
using Tzipory.Systems.UISystem;
using UnityEngine;

namespace Tzipory.GameplayLogic.UI.MetaUI.InventoryUI
{
    public class InfoText : BaseUIElement
    {
        [SerializeField] private TMP_Text _dataName;
        [SerializeField] private TMP_Text _dataValue;

        public void SetData(string dataName, string value, bool bg)
        {
            _dataName.text = dataName;
            _dataValue.text = value;
        }

    }
}