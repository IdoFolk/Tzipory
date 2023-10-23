using MyNamespaceTzipory.Systems.VisualSystem;
using Tzipory.Testing;
using UnityEngine;

namespace Tzipory.Systems.StatusSystem.EffectActionTypeSO
{
    [CreateAssetMenu(fileName = "NewPopUpEffectAction", menuName = "ScriptableObjects/VisualSystem/EffectAction/New PopUp effect action")]
    public class PopUpEffectActionConfig : BaseEffectActionConfig
    {
        public PopUpText_Config PopUpText_Config;

        public override EffectActionType ActionType => EffectActionType.PopUp;

        /// <summary>
        /// For size fix
        /// </summary>
        [SerializeField] LevelVisualDataSO _levelVisualDataSO;

#if UNITY_EDITOR
        [ContextMenu("Set Size Relative to Damage")]
        public void CallConfigSizeFix()
        {
            if(PopUpText_Config.damage <= 0)
            {
                Debug.LogError("damage amount is 0 or less - not fixing size");
                return;
            }
            PopUpText_Config.size = _levelVisualDataSO.GetRelativeFontSizeForDamage(PopUpText_Config.damage);
        }
#endif 
    }

}