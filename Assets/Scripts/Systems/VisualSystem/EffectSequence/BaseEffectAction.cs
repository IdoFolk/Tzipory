using System;
using SerializeData.VisualSystemSerializeData;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.Tools.Interface;

namespace Tzipory.VisualSystem.EffectSequence
{
    public abstract class BaseEffectAction : IInitialization<EffectActionContainerData,IEntityVisualComponent>
    {
        public event Action<BaseEffectAction> OnEffectActionComplete;
        
        private float _startDelay;
        
        private ITimer _startDelayTimer;
        private ITimer _endDelayTimer;
        private ITimer _completeTimer;
        
        public EffectActionStartType ActionStartType { get; private set; }
        public bool IsActive { get; private set; }
        
        protected abstract float Duration { get; }
        protected IEntityVisualComponent VisualComponent { get; private set; }

        private bool _disableUndo;
        private bool IsStarted { get; set; }
        
        public bool IsInitialization { get; private set; }
        
        protected BaseEffectAction()
        {
            IsActive = false;
            IsStarted = false;
            IsInitialization = false;
        }
        
        public virtual void Init(EffectActionContainerData actionContainerData, IEntityVisualComponent visualComponent)
        {
            VisualComponent = visualComponent;
            _startDelay = actionContainerData.StartDelay;
            ActionStartType = actionContainerData.EffectActionStart;
            _disableUndo = actionContainerData.DisableUndo;
            IsInitialization = true;
        }
        
        public void ActivateActionEffect()                                                           
        {
            if (!IsInitialization)
                throw  new Exception("EffectAction not initialized!");
            
            IsActive = true;                                                                         
            _startDelayTimer = VisualComponent.GameEntity.EntityTimer.StartNewTimer(_startDelay);    
        }                                                                                            

        private void StartEffectAction()
        {
            IsStarted = true;
            OnStartEffectAction();
            _completeTimer = VisualComponent.GameEntity.EntityTimer.StartNewTimer(Duration);
        }
        
        private void CompleteEffectAction()
        {
            if (!_disableUndo)
                OnUndoEffectAction();
            
            OnCompleteEffectAction();
            
            IsActive = false;
            IsStarted = false;
            
            OnEffectActionComplete?.Invoke(this);
        }

        public void InterruptEffectAction()
        {
            IsStarted = false;
            IsActive = false;
            
            VisualComponent.GameEntity.EntityTimer.StopTimer(_completeTimer);
            //need to add more logic!

            if (!_disableUndo)
                OnUndoEffectAction();

            OnInterruptEffectAction();
        }
        
        public void UpdateEffectAction()
        {
            if (!IsActive)
                return;

            if (!_startDelayTimer.IsDone)
                return;

            if (!IsStarted)
                StartEffectAction();

            OnProcessEffectAction();

            if (_completeTimer.IsDone)
                CompleteEffectAction();
        }
        
        protected T GetConfig<T>(BaseEffectActionSO effectActionSO) where T : BaseEffectActionSO
        {
            if (effectActionSO is T effectActionSo)
                return  effectActionSo;

            throw new Exception($"Can't cast {effectActionSO.GetType()} to {typeof(T)}");
        }
        
        protected abstract void OnStartEffectAction();
        protected abstract void OnProcessEffectAction();
        protected abstract void OnCompleteEffectAction();
        protected abstract void OnUndoEffectAction();
        protected abstract void OnInterruptEffectAction();
    }

    public enum EffectActionStartType
    {
        WithPrevious,
        AfterPrevious,
    }
}