using System.Collections;
using System.Collections.Generic;
using Tzipory.Systems.NodeSystem;
using UnityEngine;

namespace GameplayeLogic.Nodes
{
    [System.Serializable]
    public abstract class WorldMapNode : BaseNode
    {
        
        public override void FillInfo(BaseNode newBaseNode)
        {
            base.FillInfo(newBaseNode);
            if (newBaseNode is WorldMapNode worldMapNode)
            {
                //Add information only relvavnt to worldmap
            }
        }
    }
}
