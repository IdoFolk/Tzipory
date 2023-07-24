using System.Collections.Generic;
using UnityEngine;

namespace Systems.NodeSystem.Config
{
    [System.Serializable]
    public abstract class BaseNodeData 
    {
        public List<BaseNodeSO> UnlockNodes => new(unlockNodes);
        public List<BaseNodeSO> LeadToNodes => new(LeadToNodes);

        public string NodeName => nodeName;
        
        [SerializeField] protected string nodeName;
        [SerializeField] protected int nodeID;
        [SerializeField] protected List<BaseNodeSO> leadToNodes;
        [SerializeField] protected List<BaseNodeSO> unlockNodes;
        [SerializeField] protected bool leadAndUnlockNodesSame = true;
    }
}