using System;
using System.Collections;
using System.Collections.Generic;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace SerializeData.Nodes
{
    [System.Serializable]
    public class BattleMapNodeSerializeData : WorldMapNodeSerializeData
    {
        public BattleMapNodeState nodeState;
        public int lastBattleMoveIndex;
        public int levelID;


    }
}
