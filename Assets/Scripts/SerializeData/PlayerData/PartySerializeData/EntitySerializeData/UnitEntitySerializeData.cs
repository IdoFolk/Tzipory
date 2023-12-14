using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.ConfigFiles;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.SerializeData.StatSystemSerializeData;
using UnityEngine;

namespace Tzipory.SerializeData.PlayerData.Party.Entity
{
    [Serializable]
    public class UnitEntitySerializeData : ISerializeData , IUpdateData<UnitEntity>
    {
        [SerializeField,TabGroup("StatsId"),ReadOnly] private List<StatSerializeData> _statSerializeDatas;
        
        public List<StatSerializeData> StatSerializeDatas => _statSerializeDatas;

        public int SerializeObjectId { get; private set; }
        public int SerializeTypeId { get; private set; }

        public bool IsInitialization { get; private set; }
        
        public virtual void Init(IConfigFile parameter)
        {
            SerializeObjectId = parameter.ObjectId;
            SerializeTypeId = parameter.ConfigTypeId;
            IsInitialization = true;
        }

        public void UpdateData(UnitEntity data)
        {//may be a lot of memory waste!
           //TODO need to make a update function
        }
    }
}