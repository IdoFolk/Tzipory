using System.Collections.Generic;
using System.Linq;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.PartyConfig;
using Tzipory.ConfigFiles.PartyConfig.EntitySystemConfig;
using UnityEngine;

namespace Tzipory.ConfigFiles.WaveSystemConfig
{
    [System.Serializable]
    public class ShamanSerializeData : UnitEntitySerializeData , IUpdateData<Shaman>
    {
        public IReadOnlyList<ShamanItemSerializeData> AttachedItemsSerializeData => attachedItemsSerializeData;
        //changed to public for testing until i figure ouot that data requester
        [SerializeField,TabGroup("General"),ReadOnly] private int _shamanId;
        [SerializeField,TabGroup("General"),ReadOnly] private int _shamanLevel;
        [SerializeField,TabGroup("General"),ReadOnly] private int _shamanExp;
        
        [SerializeField] private float _decisionInterval;//temp

        //
        private List<ShamanItemSerializeData> attachedItemsSerializeData = new List<ShamanItemSerializeData>();
        //add consumables serializeData
        
        public int ShamanId => _shamanId;
        public int ShamanLevel => _shamanLevel;
        public int ShamanExp => _shamanExp;
        public float DecisionInterval => _decisionInterval;
        
        public override void Init(IConfigFile parameter)
        {
            base.Init(parameter);
            var config = (ShamanConfig)parameter;

            _decisionInterval = config.DecisionInterval;
            _shamanId = config.ConfigObjectId;
            //Need to be in config?
            attachedItemsSerializeData = new List<ShamanItemSerializeData>();
            //need to add more shaman config logic
        }

        public void UpdateData(Shaman data)
        {
            base.UpdateData(data);
            //need to add dataUpdate for shaman
        }


        public void AttachItem(ShamanItemSerializeData itemToAttach)
        {
            if (attachedItemsSerializeData.Contains(itemToAttach))
            {
                Debug.LogError("Item already attached");
                return;
            }
            for (int i = attachedItemsSerializeData.Count - 1; i >= 0; i--)
            {
                if (attachedItemsSerializeData[i].TargetSlot == itemToAttach.TargetSlot)
                {
                    RemoveItem(attachedItemsSerializeData[i]);
                }
            }
            
            attachedItemsSerializeData.Add(itemToAttach);
        }

        public void RemoveItem(ShamanItemSerializeData shamanItemSerializeData)
        {
            if (!attachedItemsSerializeData.Contains(shamanItemSerializeData))
            {
                Debug.LogError("Item does not exist for shaman! cannot remove it");
                return;
            }
            attachedItemsSerializeData.Remove(shamanItemSerializeData);
        }
    }
}