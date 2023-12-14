using Tzipory.Systems.Entity.EntityComponents;

namespace Tzipory.Systems.AbilitySystem
{
    public interface IAbilityExecutor 
    {
        public void Execute(ITargetAbleEntity target);
    }
}