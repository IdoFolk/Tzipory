using System;
using System.Collections.Generic;
using System.Linq;
using SerializeData.VisualSystemSerializeData;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.GamePlayLogic.ObjectPools;
using Tzipory.Systems.PoolSystem;
using Tzipory.Tools.Interface;

namespace Tzipory.VisualSystem.EffectSequence
{
    public class EffectSequence : IInitialization<IEntityVisualComponent,EffectSequenceData> , IPoolable<EffectSequence>
    {
        public event Action<EffectSequence> OnEffectSequenceComplete;

        #region Fields

        private float _startDelay;
        
        private List<BaseEffectAction> _effectActions;

        private IEntityVisualComponent _entityVisualComponent;
        
        private ITimer _startDelayTimer;
        
        private int _currentEffectActionIndex;

        private bool _isStarted;

        #endregion

        #region Properties

        public bool IsActive { get; private set; }


        public bool IsInterruptable { get; private set; }
        
        public string SequenceName { get; private set; }

        public int ID { get; private set; }

        public bool IsInitialization { get; private  set; }
        public bool AllEffectActionDone => _effectActions.All(effectAction => !effectAction.IsActive);
        
        #endregion

        #region PublicMethod

        public EffectSequence()
        {
            IsInitialization  = false;
        }


        public void Init(IEntityVisualComponent parameter1, EffectSequenceData parameter2)
        {
            SequenceName = parameter2.SequenceName;
            ID = parameter2.ID;
            _startDelay = parameter2.StartDelay;
            IsInterruptable = parameter2.IsInterruptable;

            IsActive = false;
            _isStarted = false;
            
            _entityVisualComponent  = parameter1;
            _effectActions = new List<BaseEffectAction>();

            foreach (var effectActionContainer in parameter2.EffectActionContainers)
            {
                BaseEffectAction effectAction = PoolManager.VisualSystemPool.GetEffectAction(effectActionContainer);
                effectAction.Init(effectActionContainer,_entityVisualComponent);
                effectAction.OnEffectActionComplete += OnActionComplete;
                _effectActions.Add(effectAction);
            }
            
            _currentEffectActionIndex = 0;
            
            IsInitialization = true;
        }
        

        #endregion

        #region PrivateMethod
        
        private void OnCompleteEffectSequence()
        {
            IsActive = false;
            
            OnEffectSequenceComplete?.Invoke(this);
            
            ResetSequence();
        }
        
        private void PlayActions()
        {
            if (_effectActions.Count == 0)
                return;
            
            var effectAction = _effectActions[_currentEffectActionIndex];
            
            if (!effectAction.IsActive)
                effectAction.ActivateActionEffect();
            
            _currentEffectActionIndex++;

            if (_currentEffectActionIndex == _effectActions.Count)
                return;

            if (_effectActions[_currentEffectActionIndex].ActionStartType == EffectActionStartType.WithPrevious)
                PlayActions();
        }
        
        private void OnActionComplete(BaseEffectAction effectAction)
        {
            if (_currentEffectActionIndex == _effectActions.Count && !_effectActions[_currentEffectActionIndex - 1].IsActive)
            {
                ResetSequence();
                return;
            }

            if (!_effectActions[_currentEffectActionIndex - 1].IsActive)
                PlayActions();
        }

        #endregion

        #region PublicMethod
        
        public void ResetSequence()
        {
            foreach (var baseEffectAction in _effectActions)
            {
                if (baseEffectAction.IsActive)
                    baseEffectAction.InterruptEffectAction();
            }
            
            _currentEffectActionIndex = 0;
            IsActive = false;
        }

        public void StartEffectSequence()
        {
            if (!IsInitialization)
                throw  new Exception("EffectSequence is not initialized");

            _startDelayTimer = _entityVisualComponent.GameEntity.EntityTimer.StartNewTimer(_startDelay);
            
            IsActive = true;
        }

        public void UpdateEffectSequence()
        {
            if (!IsActive)
                return;

            if (_startDelayTimer.IsDone && !_isStarted)
            {
                _isStarted  = true;
                PlayActions();
            }

            for (int i = 0; i < _effectActions.Count; i++)
                _effectActions[i].UpdateEffectAction();
            
            if (_currentEffectActionIndex == _effectActions.Count && AllEffectActionDone)
                OnCompleteEffectSequence();
        }

        #endregion

        // ~EffectSequence()
        // {
        //     foreach (var effectAction in _effectActions)
        //     {
        //         effectAction.OnEffectActionComplete -= OnActionComplete;
        //     }
        // }

        #region PoolObject

        public event Action<EffectSequence> OnDispose;
        public void Dispose()=>
            OnDispose?.Invoke(this);

        public void Free()
        {
            _effectActions = null;
            _startDelayTimer = null;
            _entityVisualComponent = null;
        }

        #endregion
    }
}