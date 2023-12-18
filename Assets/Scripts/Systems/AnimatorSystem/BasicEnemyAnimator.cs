using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEditor.Animations;

public class BasicEnemyAnimator : IEntityAnimatorComponent
{
    public BaseGameEntity GameEntity { get; }
    
    public AnimatorController EntityAnimatorController { get; private set; }

    public bool IsInitialization { get; }
    public void Init(BaseGameEntity parameter1, UnitEntity parameter2, AnimatorComponentConfig parameter3)
    {
        EntityAnimatorController = parameter3.EntityAnimator;
    }
    public void UpdateComponent()
    {
        throw new System.NotImplementedException();
    }

    public void Dispose()
    {
        // TODO release managed resources here
    }
}
