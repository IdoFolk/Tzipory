using UnityEngine;

namespace Tzipory.VisualSystem.EffectSequence.EffectActionTypeSO
{
    public class SpriteEffectActionConfig : BaseEffectActionConfig
    {
        public override EffectActionType ActionType => EffectActionType.Sprite;

        [SerializeField] private Sprite _sprite;
        
    }
}