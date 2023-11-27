using Tzipory.Systems.StatusSystem;
using System;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.Tools.Interface;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityHealthComponent : IEntityComponent , IStatHolder , IEntityDamageComponent ,  IInitialization<BaseGameEntity,HealthComponentConfig>
    {
        public Stat InvincibleTime { get; }
        public Stat Health { get; }
        public bool IsDamageable { get; }
        public bool IsEntityDead { get; }
        public void StartDeathSequence();
    }

    public interface IEntityDamageComponent
    {
        public Action<bool> OnHit { get; }
        public Action<Action> OnDeath { get; }
        public void Heal(float amount);
        public void TakeDamage(float damage,bool isCrit);
    }
}