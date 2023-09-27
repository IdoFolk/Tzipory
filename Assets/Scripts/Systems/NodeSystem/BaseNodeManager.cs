using Tzipory.ConfigFiles.PartyConfig.NodesConfig;
using UnityEngine;

namespace Tzipory.Systems.NodeSystem
{
    public abstract class BaseNodeManager : MonoBehaviour
    {
        public virtual void UnlockNodes(BaseNodeConfig baseNodeConfig)
        {
            foreach (BaseNodeConfig node in baseNodeConfig.UnlockNodes)
            {

            }
        }
    }
}