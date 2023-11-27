using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem.ComponentConfig
{
    [System.Serializable]
    public struct VisualComponentConfig 
    {
        [SerializeField] public Sprite Sprite;
        [SerializeField] public Sprite Icon;
    }
}