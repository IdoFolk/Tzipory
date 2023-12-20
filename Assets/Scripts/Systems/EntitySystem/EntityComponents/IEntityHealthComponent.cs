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
        public bool IsEntityDead { get; }
        public void StartDeathSequence();
    }

    public interface IEntityDamageComponent
    {
        public event Action<bool> OnHit;
        public event Action OnDeath;
        public void Heal(float amount);
        public void TakeDamage(float damage,bool isCrit);
        public void EntityDied();
    }
}