﻿using Helpers.Consts;
using SerializeData.StatSerializeData;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles;
using Tzipory.EntitySystem.StatusSystem;
using UnityEngine;

namespace Tzipory.SerializeData.StatSystemSerilazeData
{
    [System.Serializable]
    public class StatSerializeData : ISerializeData , IUpdateData<Stat>
    {
        [SerializeField,ReadOnly] private string _name;
        [SerializeField,ReadOnly] private int _id;
        [SerializeField,ReadOnly] private float _baseValue;

        public string Name => _name;

        public int ID => _id;

        public float BaseValue => _baseValue;
        
        public int SerializeTypeId => Constant.DataId.STAT_DATA_ID;

        public StatSerializeData(StatConfig statConfig)
        {
             _name  = statConfig.Name;
             _id = statConfig.Id;
             _baseValue = statConfig.BaseValue;
        }

        public StatSerializeData(Stat stat)
        {
            _name  = stat.Name;
            _id = stat.Id;
            _baseValue = stat.BaseValue;
        }

        public bool IsInitialization { get; }
        public void Init(IConfigFile parameter)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateData(Stat data)
        {
            _baseValue = data.BaseValue;
        }
    }
}