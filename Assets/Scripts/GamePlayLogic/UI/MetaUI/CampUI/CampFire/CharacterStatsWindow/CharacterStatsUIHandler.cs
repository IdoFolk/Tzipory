using System.Collections.Generic;
using Tools.Enums;
using Tzipory.ConfigFiles.Item;
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

    protected override UIGroupType GroupIndex => UIGroupType.MetaUI;
    
    public void Init(ShamanSerializeData parameter)
    {
        _serializeData  = parameter;
        
        UpdateShamanData();
        
        IsInitialization = true;
    }

    private void UpdateShamanData()
    {
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

            _statTextInfo[i].SetData(_serializeData.StatSerializeDatas[i].Name,
                $"{_serializeData.StatSerializeDatas[i].BaseValue + modifier}", bg);
            bg = !bg;
        }
    }

}