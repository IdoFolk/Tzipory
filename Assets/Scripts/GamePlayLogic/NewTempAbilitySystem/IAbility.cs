using System;
using Tzipory.Systems.StatusSystem;

namespace Tzipory.GamePlayLogic.AbilitySystem
{
    public interface IAbility : IStatHolder
    {
        public event Action OnCast;
    }
}