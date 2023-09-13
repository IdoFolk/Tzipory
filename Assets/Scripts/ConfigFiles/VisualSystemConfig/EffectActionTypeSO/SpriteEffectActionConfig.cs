using Tzipory.ConfigFiles.VisualSystemConfig;
using UnityEngine;

namespace Tzipory.ConfigFiles.VisualSystemConfig.EffectActionTypeSO
{
    public class SpriteEffectActionConfig : BaseEffectActionConfig
    {
        public override EffectActionType ActionType => EffectActionType.Sprite;

        [SerializeField] private Sprite _sprite;
        
    }
}