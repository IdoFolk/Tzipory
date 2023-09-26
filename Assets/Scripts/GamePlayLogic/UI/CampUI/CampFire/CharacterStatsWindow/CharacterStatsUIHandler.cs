using System.Globalization;
using Systems.UISystem;
using Tzipory.SerializeData;
using Tzipory.Tools.Interface;
using UnityEngine;

public class CharacterStatsUIHandler : BaseUIElement ,  IInitialization<ShamanSerializeData>
{
    [SerializeField] private InfoText[] _statTextInfo;
    
    public bool IsInitialization { get; }
    public void Init(ShamanSerializeData parameter)
    {
        for (int i = 0; i < parameter.StatSerializeDatas.Count; i++)
        {
            _statTextInfo[i].SetData(parameter.StatSerializeDatas[i].Name,parameter.StatSerializeDatas[i].BaseValue.ToString(CultureInfo.InvariantCulture));
        }
    }
}
