﻿using System.Collections.Generic;
using Tzipory.AbilitiesSystem.AbilityConfigSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.EntitySystem.StatusSystem;

namespace Tzipory.AbilitiesSystem
{
    public interface IAbilityExecutor
    {
        public AbilityExecuteType AbilityExecuteType { get; }
        public IEntityTargetAbleComponent Caster { get; }
        
        public List<StatusEffectConfigSo> StatusEffects { get; }

        public void Init(IEntityTargetAbleComponent target);
        
        public void Execute(IEntityTargetAbleComponent target);
    }
}