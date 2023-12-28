using System;
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
        public event Action OnAttack; 
        public event Action<UnitEntity> OnKill; 

        public bool Attack(ITargetAbleEntity targetAbleEntity);//may need to target parameter
        public void OnKillEvent(UnitEntity killedTarget);
    }
}