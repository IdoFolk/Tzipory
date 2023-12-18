using System;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEngine;

public class BasicEnemyAnimator : IEntityAnimatorComponent
{
    public BaseGameEntity GameEntity { get; }
    
    public RuntimeAnimatorController EntityAnimatorController { get; private set; }
    public event Action OnDeathAnimationEnd;

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
