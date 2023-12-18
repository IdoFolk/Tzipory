using System;
using Tzipory.ConfigFiles.EntitySystem.ComponentConfig;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.Tools.Interface;
using UnityEditor.Animations;
using UnityEngine;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityAnimatorComponent : IEntityComponent,IInitialization<BaseGameEntity,UnitEntity,AnimatorComponentConfig> , IDisposable
    {
        public RuntimeAnimatorController EntityAnimatorController { get; }
        public event Action OnDeathAnimationEnd;
    }
}