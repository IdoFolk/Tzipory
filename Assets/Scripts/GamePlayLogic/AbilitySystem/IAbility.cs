using System;
using Tzipory.Systems.StatusSystem;

namespace Tzipory.GamePlayLogic.AbilitySystem
{
    public interface IAbility
    {
        public event Action OnCast;
    }
}