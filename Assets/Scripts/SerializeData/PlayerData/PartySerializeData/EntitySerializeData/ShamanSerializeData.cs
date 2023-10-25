using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles;
using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.SerializeData.ItemSerializeData;
using UnityEngine;

namespace Tzipory.SerializeData.PlayerData.Party.Entity
{
    [System.Serializable]
    public class ShamanSerializeData : UnitEntitySerializeData , IUpdateData<Shaman>
    {
        //changed to public for testing until i figure ouot that data requester
        [SerializeField,TabGroup("General"),ReadOnly] private int _shamanId;
        [SerializeField,TabGroup("General"),ReadOnly] private int _shamanLevel;
        [SerializeField,TabGroup("General"),ReadOnly] private int _shamanExp;
        
        [SerializeField] private float _decisionInterval;//temp

        [SerializeField] private List<int> _itemIDList;
        
        //add consumables serializeData
        
        public int ShamanId => _shamanId;
        public int ShamanLevel => _shamanLevel;
        public int ShamanExp => _shamanExp;
        public float DecisionInterval => _decisionInterval;

        public List<int> ItemIDList => _itemIDList;

        public override void Init(IConfigFile parameter)
        {
            base.Init(parameter);
            var config = (ShamanConfig)parameter;

            _decisionInterval = config.DecisionInterval;
            _itemIDList = new List<int>();
            _shamanId = config.ObjectId;
            //Need 
            //Need to be in config?
            //need to add more shaman config logic
        }

        public void UpdateData(Shaman data)
        {
            base.UpdateData(data);
            //need to add dataUpdate for shaman
        }

        public void AddItemData(ItemContainerSerializeData itemData)
        {
            _itemIDList.Add(itemData.ItemId);
        }
    }
}