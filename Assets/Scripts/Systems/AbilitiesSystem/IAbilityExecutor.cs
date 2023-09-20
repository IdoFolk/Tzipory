﻿using System.Collections.Generic;
using Tzipory.ConfigFiles.PartyConfig.AbilitySystemConfig;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.EntitySystem.StatusSystem;

namespace Tzipory.Systems.AbilitySystem
{
    public interface IAbilityExecutor
    {
        public AbilityExecuteType AbilityExecuteType { get; }
        public IEntityTargetAbleComponent Caster { get; }
        
        public List<StatEffectConfig> OnEnterStatusEffects { get; }

        public void Init(IEntityTargetAbleComponent target);
        
        public void Execute(IEntityTargetAbleComponent target);
    }
}