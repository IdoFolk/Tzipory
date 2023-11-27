using Sirenix.OdinInspector;
using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.EntitySystem.ComponentConfig
{
    [System.Serializable]
    public struct VisualComponentConfig 
    {
        [SerializeField] public Sprite Sprite;
        [SerializeField] public Sprite Icon;
        [SerializeField] public bool UIIndicator;
        [SerializeField,ShowIf(nameof(UIIndicator))] public UIIndicatorConfig UiIndicatorConfig;
    }
}