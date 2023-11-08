using Sirenix.OdinInspector;
using Tzipory.ConfigFiles.PopUpText;
using Tzipory.Helpers;
using Tzipory.Testing;
using UnityEngine;

namespace Tzipory.Systems.VisualSystem.PopUpSystem
{
    public class PopUpTextManager : MonoSingleton<PopUpTextManager> //TODO: This will probably be integrated into LevelData TBF 
    {
        [SerializeField] private PopUpTextConfig getHitDefualtConfig;
        [SerializeField] private PopUpTextConfig getCritHitDefualtConfig;
        [SerializeField] private PopUpTextConfig healDefualtConfig;
        
        [SerializeField] public Canvas _popUpTextCanvas;
        
        [SerializeField] private LevelVisualDataSO _defaultSo;

        public Vector2 DamageRange => _defaultSo.level_VisualData.DamageRange;
        public Vector2 FontSizeRange => _defaultSo.level_VisualData.FontSizeRange;
        public float CritFontSizeBonus => _defaultSo.level_VisualData._critFontSizeBonus;
        public PopUpTextConfig DefaultPopUpConfig => _defaultSo.DefaultPopUpConfig;
        public AnimationCurve PopUpTextMoveCurve => _defaultSo.level_VisualData._popUpTextMoveCurve;
        public AnimationCurve PopUpTextScaleCurve => _defaultSo.level_VisualData._popUpTextScaleCurve;
        public AnimationCurve PopUpTextAlphaCurve => _defaultSo.level_VisualData._popUpTextAlphaCurve;
        
        public PopUpTextConfig GetHitDefaultConfig => getHitDefualtConfig;
        public PopUpTextConfig GetCritHitDefaultConfig => getCritHitDefualtConfig;
        public PopUpTextConfig HealDefaultConfig => healDefualtConfig;
        
        public float GetRelativeFontSizeForDamage(float damage) => Mathf.Clamp( (FontSizeRange.y - FontSizeRange.x) * (damage - DamageRange.x) / (DamageRange.y - DamageRange.x) + FontSizeRange.x, FontSizeRange.x, FontSizeRange.y);
        
    }

    [System.Serializable]
    public struct LevelVisualData
    {
        [MinMaxSlider(1,1000)] public Vector2 DamageRange;
        [MinMaxSlider(1,100)] public Vector2 FontSizeRange;
        public float _critFontSizeBonus;

        public AnimationCurve _popUpTextMoveCurve;
        public AnimationCurve _popUpTextScaleCurve;
        public AnimationCurve _popUpTextAlphaCurve;
    }
}