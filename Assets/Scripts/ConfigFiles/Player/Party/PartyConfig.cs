﻿using Tzipory.ConfigFiles.EntitySystem;
using Tzipory.Helpers.Consts;
using UnityEngine;

namespace Tzipory.ConfigFiles.Player.Party
{
    [System.Serializable]
    public class PartyConfig : IConfigFile
    {
        [SerializeField] private UnitEntityConfig[] _partyMembers;

        public UnitEntityConfig[] PartyMembers => _partyMembers;

        public int ObjectId => 0;
        
        public int ConfigTypeId => Constant.DataId.PARTY_DATA_ID;
    }
}