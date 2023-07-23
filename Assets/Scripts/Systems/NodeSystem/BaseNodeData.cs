using System.Collections.Generic;
using UnityEngine;

namespace Systems.NodeSystem.Config
{
    [System.Serializable]
    public abstract class BaseNodeData 
    {
        [SerializeField] protected string nodeID;
        [SerializeField] protected List<BaseNodeSO> leadToNodes;
        [SerializeField] protected List<BaseNodeSO> unlockNodes;
        [SerializeField] protected bool leadAndUnlockNodesSame = true;
    }
}