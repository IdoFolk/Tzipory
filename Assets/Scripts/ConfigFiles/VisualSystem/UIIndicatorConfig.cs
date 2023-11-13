using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.Systems.StatusSystem
{
    [System.Serializable]
    public struct UIIndicatorConfig
    {
        [SerializeField] public Sprite Image;
        [SerializeField] public Color Color;
        [SerializeField] public bool AllwaysShow;
        [SerializeField] public bool DisposOnClick;
        [SerializeField] public float OffSetRadios;
        [SerializeField] public bool StartFlashing;
        [SerializeField,ShowIf(nameof(StartFlashing))] public UIIndicatorFlashConfig FlashConfig;
    }

    [System.Serializable]
    public struct UIIndicatorFlashConfig
    {
        [SerializeField] public float SizeFactor;
        [SerializeField] public float FlashSpeed;
        [SerializeField] public bool UseTime;
        [SerializeField,ShowIf(nameof(UseTime))] public float Time;
        [SerializeField] public bool OverrideFlashingColor;
        [SerializeField,ShowIf(nameof(OverrideFlashingColor))] public Color FlashingColor;
    }
}