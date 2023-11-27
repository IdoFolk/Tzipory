using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;

namespace Tzipory.GamePlayLogic.EntitySystem.EntityComponent
{
    public class ExperienceComponent : IEntityExperienceComponent
    {
        #region Proprty

        public float EntityExperience { get; private set; }
        public int EntityLevel { get; private set; }
        
        public BaseGameEntity GameEntity { get; private set; }
        
        public bool IsInitialization { get; private set; }

        #endregion

        #region Init

        public void Init(BaseGameEntity parameter)
        {
            GameEntity = parameter;
            EntityLevel = 0;
            EntityExperience = 0;
        }

        #endregion

        #region PublicMethod

        public void UpdateComponent()
        {
        }

        public void AddExperience(float experience)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}