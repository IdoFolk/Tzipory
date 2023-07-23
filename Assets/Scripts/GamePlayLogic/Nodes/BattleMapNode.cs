using System;
using System.Collections;
using System.Collections.Generic;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace GameplayeLogic.Nodes
{
    [System.Serializable]
    public class BattleMapNode : WorldMapNode
    {
        public BattleMapNodeState nodeState;
        public DateTime lastBattleTime;
    }
}
