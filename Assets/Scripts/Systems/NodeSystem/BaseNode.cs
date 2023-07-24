using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tzipory.Systems.NodeSystem
{
    [System.Serializable]
    public abstract class BaseNode
    {
        public string NodeID => nodeID;
        
        private string nodeID;
        private bool visitedByPlayer;
        
        public virtual void FillInfo(BaseNode newBaseNode)
        {
            nodeID = newBaseNode.NodeID;
        }
    }
}
