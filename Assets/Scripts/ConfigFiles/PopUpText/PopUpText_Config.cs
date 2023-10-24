using Sirenix.OdinInspector;
using Tzipory.Systems.VisualSystem.PopUpSystem;
using UnityEngine;

namespace Tzipory.ConfigFiles.PopUpText
{
    [System.Serializable]
    public struct PopUpTextConfig
    {
        public PopUpTextType PopUpTextType;
        [ShowIf("PopUpTextType",PopUpTextType.ShowText)] public string Text;
        public Color Color;
        public bool OverrideSize;
        [ShowIf("OverrideSize")] public float StartSize;
        public float RiseSpeed;
        /// <summary>
        /// In seconds
        /// </summary>
        public float TimeToLive;

        public bool OverrideAnimationCurve;
        public bool AddRandomMoveOffSet;
        [ShowIf("AddRandomMoveOffSet")] [MinMaxSlider(0,5)] [SerializeField] public Vector2 MoveOffSet;
        [ShowIf("OverrideAnimationCurve")] public AnimationCurve PopUpTextMoveCurve;
        [ShowIf("OverrideAnimationCurve")] public AnimationCurve PopUpTextScaleCurve;
        [ShowIf("OverrideAnimationCurve")] public AnimationCurve PopUpTextAlphaCurve;

        public void SetText(int text)
        {
            Text = text.ToString();
            StartSize = PopUpTextManager.Instance.GetRelativeFontSizeForDamage(text);
        }
        
        public void SetText(string text,float size)
        {
            Text = text;
            StartSize = size;
        }
    }

    public enum PopUpTextType
    {
        ShowDelta,
        ShowNewValue,
        ShowText
    }
}