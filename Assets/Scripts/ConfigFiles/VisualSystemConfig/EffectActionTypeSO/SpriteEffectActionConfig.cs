using Tzipory.GameplayLogic.StatusEffectTypes;
using UnityEngine;

namespace Tzipory.GameplayLogic.StatusEffectTypes.EffectActionTypeSO
{
    public class SpriteEffectActionConfig : BaseEffectActionConfig
    {
        public override EffectActionType ActionType => EffectActionType.Sprite;

        [SerializeField] private Sprite _sprite;
        
    }
}