using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.Systems.StatusSystem;
using Tzipory.Tools.Interface;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityCombatComponent : IEntityComponent , IInitialization<BaseGameEntity,CombatComponentConfig> ,IStatHolder
    {
        public Stat AttackDamage { get; }
        public Stat CritDamage { get; }
        public Stat CritChance { get; }
        public Stat AttackRate { get; }
        public Stat AttackRange { get; }

        public void Attack();//may need to target parameter
    }
}