using System.Collections;
using System.Collections.Generic;
using Tzipory.Nodes.Config;
using UnityEngine;

public abstract class BaseNodeManager : MonoBehaviour
{
    public virtual void UnlockNodes(BaseNodeConfig baseNodeConfig)
    {
        foreach (BaseNodeConfig node in baseNodeConfig.UnlockNodes)
        {
            
        }
    }
}
