using TMPro;
using Tools.Enums;
using Tzipory.Systems.UISystem;
using UnityEngine;

public class InfoText : BaseUIElement
{
    [SerializeField] private TMP_Text _dataName;
    [SerializeField] private TMP_Text _dataValue;
    protected override UIGroupType GroupIndex => UIGroupType.MetaUI;

    public void SetData(string dataName, string value,bool bg)
    {
        _dataName.text = dataName;
        _dataValue.text = value;
    }

}
