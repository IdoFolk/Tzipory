using System;
using System.Collections.Generic;
using System.Linq;
using Tzipory.ConfigFiles.Item;
using Tzipory.GameplayLogic.UI.MetaUI.InventoryUI;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Systems.DataManager;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.Interface;
using UnityEngine;

public class CharacterStatsUIHandler : BaseUIElement ,  IInitialization<ShamanSerializeData>
{
    [SerializeField] private InfoText[] _statTextInfo;
    public bool IsInitialization { get; private set; }
    
    private ShamanSerializeData _serializeData;
    
    public void Init(ShamanSerializeData parameter)
    {
        _serializeData  = parameter;
        
        UpdateUIVisual();
        
        IsInitialization = true;
    }

    public override void UpdateUIVisual()
    {
        base.UpdateUIVisual();
        bool bg = true;
        
        List<ItemConfig> itemConfigs = new List<ItemConfig>(_serializeData.ItemIDList.Count);

        foreach (var itemID in _serializeData.ItemIDList)
            itemConfigs.Add(DataManager.DataRequester.GetConfigData<ItemConfig>(itemID));

        for (int i = 0; i < _serializeData.StatSerializeDatas.Count; i++)
        {
            float modifier = 0;

            foreach (var itemConfig in itemConfigs)
            {
                foreach (var effectConfig in itemConfig.StatEffectConfigs)
                {
                    if (effectConfig.AffectedStatId == _serializeData.StatSerializeDatas[i].ID)
                    {
                        modifier += effectConfig.StatModifier.Modifier;
                    }
                }
            }

            //SetStatData(i, _serializeData.StatSerializeDatas[i].Name, _serializeData.StatSerializeDatas[i].BaseValue, modifier, bg);
            bg = !bg;
        }
    }

    public override void Show()
    {
        foreach (var infoText in _statTextInfo)
            infoText.Show();
        base.Show();
    }

    private void SetStatData(int i,string dataName,float baseValue,float modifier, bool bg)
    {
        string newDataName = string.Concat(dataName.Select(x => Char.IsUpper(x) ? " " + x : x.ToString())).TrimStart(' ');
        string newDataValue;
        if (modifier > 0) newDataValue = $"{baseValue} + {modifier}";
        else if (modifier < 0) newDataValue = $"{baseValue} - {modifier}";
        else newDataValue = $"{baseValue}";
       
        _statTextInfo[i].SetData(newDataName, newDataValue, bg);
    }
}