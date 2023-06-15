﻿using Tzipory.EntitySystem.StatusSystem;

namespace Tzipory.EntitySystem.EntityComponents
{
    public interface IEntityHealthComponent : IEntityComponent
    {
        public Stat InvincibleTime { get; }
        public bool IsDamageable { get; }
        public Stat HP { get; }
        public bool IsEntityDead { get; }
        
        public void Heal(float amount);
        
        public void TakeDamage(float damage,bool isCrit);
    }
}