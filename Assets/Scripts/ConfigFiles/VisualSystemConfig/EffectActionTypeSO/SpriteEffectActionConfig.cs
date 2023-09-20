using Tzipory.Systems.StatusSystem;
using UnityEngine;

namespace Tzipory.Systems.StatusSystem.EffectActionTypeSO
{
    public class SpriteEffectActionConfig : BaseEffectActionConfig
    {
        public override EffectActionType ActionType => EffectActionType.Sprite;

        [SerializeField] private Sprite _sprite;
        
    }
}