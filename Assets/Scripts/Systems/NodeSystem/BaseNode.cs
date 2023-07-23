using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tzipory.Systems.NodeSystem
{
    [System.Serializable]
    public abstract class BaseNode
    {
        public string nodeID;
        public List<BaseNode> leadToNodes;
        public List<BaseNode> unlockNodes;
        public bool leadAndUnlockNodesSame = true;
    }
}
