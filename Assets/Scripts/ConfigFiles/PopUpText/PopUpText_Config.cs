using Sirenix.OdinInspector;
using Tzipory.Editor;
using Tzipory.Systems.VisualSystem.PopUpSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.PopUpText
{
    [System.Serializable]
    public struct PopUpTextConfig
    {
        public static string DeltaKeyCode = "{Delta}";
        public static string ModifierKeyCode = "{Modifier}";
        public static string NewValueKeyCode = "{NewValue}";
        public static string NameKeyCode = "{Name}";
        
        public bool DisablePopUp;
        [HideIf("DisablePopUp")] public PopUpTextType PopUpTextType;
        [HideIf("DisablePopUp")] public TextSpawnRepeatPatterns RepeatPattern;
        [HideIf("DisablePopUp")] [ShowIf("PopUpTextType",PopUpTextType.ShowText),TextArea(4,4),ColorfulString] public string Text;
        [HideIf("DisablePopUp")] public Color Color;
        [HideIf("DisablePopUp")] public bool OverrideSize;
        [HideIf("DisablePopUp")] [ShowIf("OverrideSize")] public float FontSize;
        [HideIf("DisablePopUp")] public bool OverrideRiseSpeedAndTTL;
        [HideIf("DisablePopUp"),ShowIf("OverrideRiseSpeedAndTTL")] public float RiseSpeed;
        [HideIf("DisablePopUp"),ShowIf("OverrideRiseSpeedAndTTL")] public float TimeToLive;
        [HideIf("DisablePopUp")] public bool OverrideStartSize;
        [HideIf("DisablePopUp"),ShowIf("OverrideStartSize")] public Vector2 StartSize;
        [HideIf("DisablePopUp")] public bool AddRandomMoveOffSet;
        [HideIf("DisablePopUp")] [ShowIf("AddRandomMoveOffSet")] [MinMaxSlider(0,5)] [SerializeField] public Vector2 MoveOffSet;
        [HideIf("DisablePopUp")] public bool OverrideAnimationCurve;
        [HideIf("DisablePopUp")] [ShowIf("OverrideAnimationCurve")] public AnimationCurve PopUpTextMoveCurve;
        [HideIf("DisablePopUp")] [ShowIf("OverrideAnimationCurve")] public AnimationCurve PopUpTextScaleCurve;
        [HideIf("DisablePopUp")] [ShowIf("OverrideAnimationCurve")] public AnimationCurve PopUpTextAlphaCurve;
    }

    public enum PopUpTextType
    {
        ShowName,
        ShowDelta,
        ShowNewValue,
        ShowText,
        ShowModifier
    }
}