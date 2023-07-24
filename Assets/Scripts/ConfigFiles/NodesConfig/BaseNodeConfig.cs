using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tzipory.Nodes.Config
{
    [CreateAssetMenu(fileName = "BaseNode", menuName = "ScriptableObjects/Nodes/BaseNode", order = 1)]
    public class BaseNodeConfig : ScriptableObject
    {
        public List<BaseNodeConfig> UnlockNodes => new(unlockNodes);
        public List<BaseNodeConfig> LeadToNodes => new(LeadToNodes);

        public string NodeName => nodeName;
        
        [SerializeField] protected string nodeName;
        [SerializeField] protected int nodeID;
        [SerializeField] protected List<BaseNodeConfig> leadToNodes;
        [SerializeField] protected List<BaseNodeConfig> unlockNodes;
        [SerializeField] protected bool leadAndUnlockNodesSame = true;
    }
}
