using System.Collections;
using System.Collections.Generic;
using Systems.NodeSystem.Config;
using Tzipory.Systems.NodeSystem;
using UnityEngine;

public abstract class BaseNodeManager : MonoBehaviour
{
    public virtual void UnlockNodes(BaseNodeData baseNodeData)
    {
        foreach (BaseNodeSO node in baseNodeData.UnlockNodes)
        {
            
        }
    }
}
