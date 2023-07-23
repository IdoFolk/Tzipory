using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tzipory.Systems.NodeSystem
{
    [System.Serializable]
    public abstract class BaseNode
    {
        public string nodeID;

        public virtual void FillInfo(BaseNode newBaseNode)
        {
            nodeID = newBaseNode.nodeID;
        }
    }
}
