using System;
using SerializeData.VisualSystemSerializeData;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.Tools.Interface;

namespace Tzipory.VisualSystem.EffectSequence
{
    public abstract class BaseEffectAction : IInitialization<EffectActionContainerConfig,IEntityVisualComponent>
    {
        public event Action<BaseEffectAction> OnEffectActionComplete;
        
        #region Proprty

        public EffectActionStartType ActionStartType { get; private set; }
        protected IEntityVisualComponent VisualComponent { get; private set; }
        public abstract float Duration { get; }
        public bool DisableUndo { get; private set; }
        public bool IsInitialization { get; private set; }
        
        #endregion

        protected BaseEffectAction()
        {
            IsInitialization = false;
        }

        #region PublicMethod

        public virtual void Init(EffectActionContainerConfig actionContainerConfig, IEntityVisualComponent visualComponent)
        {
            VisualComponent = visualComponent;
            ActionStartType = actionContainerConfig.EffectActionStart;
            DisableUndo = actionContainerConfig.DisableUndo;
            IsInitialization = true;
        }

        #endregion

        #region PrivateMethod

        protected T GetConfig<T>(BaseEffectActionConfig effectActionConfig) where T : BaseEffectActionConfig
        {
            if (effectActionConfig is T effectActionSo)
                return  effectActionSo;

            throw new Exception($"Can't cast {effectActionConfig.GetType()} to {typeof(T)}");
        }
        
        #endregion

        #region abstractMethod

        public abstract void UndoEffectAction();
        public abstract void StartEffectAction();
        public abstract void ProcessEffectAction();
        public abstract void CompleteEffectAction();
        public abstract void InterruptEffectAction();

        #endregion
    }

    public enum EffectActionStartType
    {
        WithPrevious,
        AfterPrevious,
    }
}