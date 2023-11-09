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
        
        [SerializeField] private LevelVisualDataSO _defualtSo;
        private LevelVisualData _visualData;

        public Vector2 DamageRange => _visualData.DamageRange;
        public Vector2 FontSizeRange => _visualData.FontSizeRange;
        public float CritFontSizeBonus => _visualData._critFontSizeBonus;
        public PopUpTextConfig DefaultPopUpConfig => _defualtSo.DefaultPopUpConfig;
        public AnimationCurve PopUpTextMoveCurve => _visualData._popUpTextMoveCurve;
        public AnimationCurve PopUpTextScaleCurve => _visualData._popUpTextScaleCurve;
        public AnimationCurve PopUpTextAlphaCurve => _visualData._popUpTextAlphaCurve;
        
        public PopUpTextConfig GetHitDefaultConfig => getHitDefualtConfig;
        public PopUpTextConfig GetCritHitDefaultConfig => getCritHitDefualtConfig;
        public PopUpTextConfig HealDefaultConfig => healDefualtConfig;
        
        public float GetRelativeFontSizeForDamage(float damage) => Mathf.Clamp( (FontSizeRange.y - FontSizeRange.x) * (damage - DamageRange.x) / (DamageRange.y - DamageRange.x) + FontSizeRange.x, FontSizeRange.x, FontSizeRange.y);

        public override void Awake()
        {
            base.Awake();
            _visualData = _defualtSo.level_VisualData;
        }

        public void SetLevelVisualDataSO(LevelVisualDataSO levelVisualDataSO)
        {
            _visualData = levelVisualDataSO.level_VisualData;
        }
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