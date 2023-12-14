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
        [SerializeField] public bool HpBar;
        [SerializeField] public bool HaveSilhouette;
        
        [SerializeField, TabGroup("UI Indicator")] public bool UIIndicator;
        [SerializeField, ShowIf(nameof(UIIndicator)), TabGroup("UI Indicator")]
        public UIIndicatorConfig UiIndicatorConfig;

        [SerializeField, TabGroup("Visual Events")]
        public EffectSequenceConfig OnDeath;

        [SerializeField, TabGroup("Visual Events")]
        public EffectSequenceConfig OnAttack;

        [SerializeField, TabGroup("Visual Events")]
        public EffectSequenceConfig OnCritAttack;

        [SerializeField, TabGroup("Visual Events")]
        public EffectSequenceConfig OnMove;

        [SerializeField, TabGroup("Visual Events")]
        public EffectSequenceConfig OnGetHit;

        [SerializeField, TabGroup("Visual Events")]
        public EffectSequenceConfig OnGetCritHit;
    }
}