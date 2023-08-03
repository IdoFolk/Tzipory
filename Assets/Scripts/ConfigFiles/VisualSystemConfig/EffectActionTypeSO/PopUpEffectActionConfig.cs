using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;


namespace Tzipory.VisualSystem.EffectSequence.EffectActionTypeSO
{
    [CreateAssetMenu(fileName = "NewPopUpEffectAction", menuName = "ScriptableObjects/VisualSystem/EffectAction/New PopUp effect action")]
    public class PopUpEffectActionConfig : BaseEffectActionConfig
    {
        public PopUpText_Config PopUpText_Config;

        public override EffectActionType ActionType => EffectActionType.PopUp;

#if UNITY_EDITOR
        [ContextMenu("Set Size Relative to Damage")]
        public void CallConfigSizeFix()
        {
            PopUpText_Config.SetSizeRelativeToDamage();
        }
#endif 
    }

}