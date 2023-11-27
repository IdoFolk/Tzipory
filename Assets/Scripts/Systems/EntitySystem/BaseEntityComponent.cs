using Tzipory.ConfigFiles;
using Tzipory.Systems.Entity;

namespace Tzipory.Systems.EntitySystem
{
    public abstract class BaseEntityComponent : IEntityComponent
    {
        public BaseGameEntity GameEntity { get; private set; }

        protected BaseEntityComponent(BaseGameEntity baseGameEntity,IConfigFile configFile)
        {
            GameEntity = baseGameEntity; 
        }

        public abstract void UpdateComponent();
        public bool IsInitialization { get; }
        public void Init(BaseGameEntity parameter)
        {
            throw new System.NotImplementedException();
        }
    }
}