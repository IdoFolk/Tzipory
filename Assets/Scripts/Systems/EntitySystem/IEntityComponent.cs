using Tzipory.Tools.Interface;

namespace Tzipory.Systems.Entity
{
    public interface IEntityComponent :  IEntity
    {
        public void UpdateComponent();
    }
    
    public interface IEntity
    {
        public BaseGameEntity GameEntity { get; }
    }
}