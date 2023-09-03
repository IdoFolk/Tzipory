﻿using System;
using System.Collections.Generic;
using SerializeData.VisualSystemSerializeData;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.GamePlayLogic.ObjectPools;
using Tzipory.Systems.PoolSystem;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.VisualSystem.EffectSequence
{
    public class EffectSequence : IInitialization<IEntityVisualComponent,EffectSequenceConfig,Action> , IPoolable<EffectSequence>
    {
        #region Fields

        private Action _onComplete;
        
        private float _startDelay;
        
        //private List<BaseEffectAction> _activeEffectActions;

        private Dictionary<ITimer, BaseEffectAction> _startEffectActions;
        private Dictionary<ITimer, BaseEffectAction> _activeEffectActions;

        private EffectActionContainerConfig[] _effectActionConfigs;

        private IEntityVisualComponent _entityVisualComponent;
        
        private ITimer _startDelayTimer;
        
        private int _currentEffectActionIndex;

        private bool _isStarted;

        #endregion

        #region Properties



        public bool IsInterruptable { get; private set; }
        
        public string SequenceName { get; private set; }

        public int ID { get; private set; }

        public bool IsInitialization { get; private  set; }
        public bool AllEffectActionDone => _activeEffectActions.Count == 0 && _startEffectActions.Count == 0 && _isStarted; 
        
        #endregion

        #region PublicMethod

        public EffectSequence()
        {
            IsInitialization  = false;
            //_activeEffectActions = new List<BaseEffectAction>();
            _startEffectActions = new Dictionary<ITimer, BaseEffectAction>();
            _activeEffectActions = new Dictionary<ITimer, BaseEffectAction>();
        }

        public void Init(IEntityVisualComponent parameter1, EffectSequenceConfig parameter2,Action onComplete = null)
        {
            SequenceName = parameter2.SequenceName;
            ID = parameter2.ID;
            IsInterruptable = parameter2.IsInterruptable;

            _onComplete = onComplete;
            
            _isStarted = false;
            _entityVisualComponent = parameter1;

            _effectActionConfigs = parameter2.EffectActionContainers;
            
            _currentEffectActionIndex = 0;
            
            IsInitialization = true;
            
            _startDelayTimer = _entityVisualComponent.GameEntity.EntityTimer.StartNewTimer(parameter2.StartDelay,$"EffectSequence: {SequenceName} start Delay Timer");
        }
        
        #endregion

        #region PrivateMethod
        
        private void OnCompleteEffectSequence()
        {
            _onComplete?.Invoke();
#if UNITY_EDITOR
            Debug.Log($"<color=#fc6b03>Effect Handler:</color> sequence {SequenceName} as completed on entity <color=#a903fc>{_entityVisualComponent.GameEntity.name}</color>");
#endif
            Dispose();
        }
        
        private void PlayActions()
        {
            if (_effectActionConfigs.Length == 0)
                return;
            
            var effectActionConfig = _effectActionConfigs[_currentEffectActionIndex];
            
            BaseEffectAction effectAction = PoolManager.VisualSystemPool.GetEffectAction(effectActionConfig);
            effectAction.Init(effectActionConfig,_entityVisualComponent);

            ITimer effectActionStartDelayTimer =
                _entityVisualComponent.GameEntity.EntityTimer.StartNewTimer(effectActionConfig.StartDelay,
                    "Effect action start delay timer");
            
            _startEffectActions.Add(effectActionStartDelayTimer,effectAction);
            
            effectActionStartDelayTimer.OnTimerComplete += OnActionTimerComplete;
            
            if (_currentEffectActionIndex == _effectActionConfigs.Length - 1)
                return;
            
            _currentEffectActionIndex++;
            
            if (_effectActionConfigs[_currentEffectActionIndex].EffectActionStart == EffectActionStartType.WithPrevious)
                PlayActions();
        }
        
        private void OnActionTimerComplete(ITimer effectActionTime,bool isTimerStopped)
        {
            if (_startEffectActions.TryGetValue(effectActionTime,out var startEffectAction))
            {
                startEffectAction.StartEffectAction();
                    
                effectActionTime.OnTimerComplete -= OnActionTimerComplete;
                _startEffectActions.Remove(effectActionTime);

                ITimer durationTimer =
                    _entityVisualComponent.GameEntity.EntityTimer.StartNewTimer(startEffectAction.Duration,"Effect action duration");

                durationTimer.OnTimerComplete += OnActionTimerComplete;
                
                _activeEffectActions.Add(durationTimer,startEffectAction);
                
                return;
            }
            
            if (_activeEffectActions.TryGetValue(effectActionTime,out var activeEffectAction))
            {
                if (activeEffectAction.DisableUndo)
                {
                    activeEffectAction.CompleteEffectAction();
                }
                else
                {
                    activeEffectAction.UndoEffectAction();
                    activeEffectAction.CompleteEffectAction();
                }

                effectActionTime.OnTimerComplete -= OnActionTimerComplete;
                _activeEffectActions.Remove(effectActionTime);
            }

            if (AllEffectActionDone)
                OnCompleteEffectSequence();
        }

        #endregion

        #region PublicMethod
        
        public void ResetSequence()//not in use
        {
            foreach (var baseEffectAction in _startEffectActions)
                baseEffectAction.Value.InterruptEffectAction();
            
            foreach (var baseEffectAction in _activeEffectActions)
                baseEffectAction.Value.InterruptEffectAction();

            _currentEffectActionIndex = 0;
        }

        public void UpdateEffectSequence()
        {
            if (!IsInitialization || _isStarted)
                return;

            if (_startDelayTimer.IsDone)
            {
                PlayActions();
                _isStarted  = true;
            }
        }

        #endregion

        #region PoolObject

        public event Action<EffectSequence> OnDispose;

        public void Dispose()
        {
            // foreach (var baseEffectAction in _activeEffectActions)
            // {
            //     if (baseEffectAction.IsActive && !baseEffectAction.DisableUndo)
            //         baseEffectAction.InterruptEffectAction();
            // }
            _onComplete = null;
            _activeEffectActions.Clear();
            _startDelayTimer = null;
            IsInitialization = false;
            OnDispose?.Invoke(this);
        }

        public void Free()
        {
            _onComplete = null;
            _activeEffectActions = null;
            _startDelayTimer = null;
            _entityVisualComponent = null;
        }

        #endregion
    }
}