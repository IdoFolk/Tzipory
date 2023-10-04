using Tzipory.Systems.StatusSystem;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityCombatComponent : IEntityComponent
    {
        public Stat AttackDamage { get; }
        public Stat CritDamage { get; }
        public Stat CritChance { get; }
        public Stat AttackRate { get; }
        public Stat AttackRange { get; }

        public void Attack();//may need to target parameter
    }
}