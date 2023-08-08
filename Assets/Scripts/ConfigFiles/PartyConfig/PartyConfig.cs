﻿using Tzipory.EntitySystem.EntityConfigSystem;
using UnityEngine;
using Constant = Helpers.Consts.Constant;

namespace Tzipory.ConfigFiles
{
    [CreateAssetMenu(fileName = "NewPartyConfig", menuName = "ScriptableObjects/Config/New party config", order = 0)]
    public class PartyConfig : ScriptableObject , IConfigFile
    {
        [SerializeField] private int _configObjectId;
        [SerializeField] private ShamanConfig[] _partyMembers;

        public ShamanConfig[] PartyMembers => _partyMembers;

        public int ConfigObjectId => _configObjectId;
        
        public int ConfigTypeId => Constant.DataId.PARTY_DATA_ID;
    }
}