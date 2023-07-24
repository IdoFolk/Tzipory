using System;
using System.Collections;
using System.Collections.Generic;
using Tzipory.Systems.NodeSystem;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace GameplayeLogic.Nodes
{
    [System.Serializable]
    public class BattleMapNode : WorldMapNode
    {
        public BattleMapNodeState nodeState;
        public int lastBattleMoveIndex;
        public int levelID;

        public override void FillInfo(BaseNode newBaseNode)
        {
            base.FillInfo(newBaseNode);
            if (newBaseNode is BattleMapNode battleMapNodeMapNode)
            {
                nodeState = battleMapNodeMapNode.nodeState;
                lastBattleMoveIndex = battleMapNodeMapNode.lastBattleMoveIndex;
            }
        }
    }
}
