﻿using Tzipory.EntitySystem.StatusSystem;

namespace Tzipory.EntitySystem.EntityComponents
{
    public interface IEntityCombatComponent : IEntityComponent
    {
        public IEntityTargetAbleComponent Target { get; }
        
        public Stat AttackDamage { get; }
        public Stat CritDamage { get; }
        public Stat CritChance { get; }
        public Stat AttackRate { get; }
        public Stat AttackRange { get; }

        public void Attack();
    }
}