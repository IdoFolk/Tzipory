using Tzipory.ConfigFiles.VisualSystemConfig;

namespace Tzipory.EntitySystem.EntityComponents
{
    public interface IEntityHealthComponent : IEntityComponent
    {
        public Stat InvincibleTime { get; }
        public Stat Health { get; }
        public bool IsDamageable { get; }
        public bool IsEntityDead { get; }
        
        public void Heal(float amount);
        public void TakeDamage(float damage,bool isCrit);
        public void StartDeathSequence();
    }
}