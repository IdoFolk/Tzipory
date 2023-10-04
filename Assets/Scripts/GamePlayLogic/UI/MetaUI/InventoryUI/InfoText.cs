using Systems.UISystem;
using TMPro;
using UnityEngine;

public class InfoText : BaseUIElement
{
    [SerializeField] private TMP_Text _dataName;
    [SerializeField] private TMP_Text _dataValue;

    public void SetData(string dataName, string value,bool bg)
    {
        _dataName.text = dataName;
        _dataValue.text = value;
    }
}
