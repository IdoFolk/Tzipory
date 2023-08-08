﻿using System.Collections.Generic;
using Shamans;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles;
using Tzipory.EntitySystem.EntityConfigSystem;
using UnityEngine;

namespace Tzipory.SerializeData
{
    [System.Serializable]
    public class ShamanSerializeData : UnitEntitySerializeData , IUpdateData<Shaman>
    {
        public IReadOnlyList<ShamanItemSerializeData> ItemsSerializeData => itemsSerializeData;
        //changed to public for testing until i figure ouot that data requester
        [SerializeField,TabGroup("General"),ReadOnly] private int _shamanId;
        [SerializeField,TabGroup("General"),ReadOnly] private int _shamanLevel;
        [SerializeField,TabGroup("General"),ReadOnly] private int _shamanExp;

        //
        private List<ShamanItemSerializeData> itemsSerializeData = new List<ShamanItemSerializeData>();
        //add consumables serializeData
        
        public int ShamanId => _shamanId;
        public int ShamanLevel => _shamanLevel;
        public int ShamanExp => _shamanExp;
        
        public override void Init(IConfigFile parameter)
        {
            base.Init(parameter);
            var config = (ShamanConfig)parameter;
            
            _shamanId = config.ConfigObjectId;
            //Need to be in config?
            itemsSerializeData = new List<ShamanItemSerializeData>();
            //need to add more shaman config logic
        }

        public void UpdateData(Shaman data)
        {
            base.UpdateData(data);
            //need to add dataUpdate for shaman
        }


        public void AttachItem(ShamanItemSerializeData itemToAttach)
        {
            for (int i = itemsSerializeData.Count - 1; i >= 0; i--)
            {
                if (itemsSerializeData[i].TargetSlot == itemToAttach.TargetSlot)
                {
                    RemoveItem(itemsSerializeData[i]);
                }
            }
            
            itemsSerializeData.Add(itemToAttach);
        }

        public void RemoveItem(ShamanItemSerializeData shamanItemSerializeData)
        {
            itemsSerializeData.Remove(shamanItemSerializeData);
        }
    }
}