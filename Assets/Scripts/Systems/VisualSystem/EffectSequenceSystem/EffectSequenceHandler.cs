using System;
using System.Collections.Generic;
using SerializeData.VisualSystemSerializeData;
using Tzipory.EntitySystem.EntityComponents;
using Tzipory.GamePlayLogic.ObjectPools;
using UnityEngine;

namespace Tzipory.VisualSystem.EffectSequence
{
    public class EffectSequenceHandler
    {
        private readonly Dictionary<int, EffectSequenceConfig> _sequencesDictionary;

        private readonly List<EffectSequence> _activeSequences;

        private readonly IEntityVisualComponent _entityVisualComponent;

        public EffectSequenceHandler(IEntityVisualComponent visualComponent,IEnumerable<EffectSequenceConfig> sequencesDatas)
        {
            _sequencesDictionary = new();
            _activeSequences = new();
            
            _entityVisualComponent = visualComponent;

            foreach (var sequenceData in sequencesDatas)
                _sequencesDictionary.Add(sequenceData.ID, sequenceData);
        }

        #region PublicMethod

        public void PlaySequenceById(int id,Action onComplete = null)
        {
            if (!_sequencesDictionary.TryGetValue(id, out var effectSequenceData))
            {
                Debug.LogWarning($"Sequence with id {id} not found");
                return;
            }
            
            PlaySequence(effectSequenceData,onComplete);
        }
        
        public void PlaySequenceByData(EffectSequenceConfig effectSequenceConfig,Action onComplete = null)
        {
            if (effectSequenceConfig.EffectActionContainers.Length == 0)
                return;
            
            PlaySequence(effectSequenceConfig,onComplete);
        }

        public void RemoveEffectSequence(int effectSequenceId)
        {
            if (_activeSequences.Count == 0)
                return;

            foreach (var effectSequence in _activeSequences)
            {
                if (effectSequence.ID == effectSequenceId)
                {
                    RemoveEffectSequence(effectSequence);
                    return;//can only remove one 
                }
            }
            
            Debug.LogWarning($"Try remove a effectSequence that not exists effect ID : {effectSequenceId}");
        }
        
        public void UpdateEffectHandler()
        {
            for (int i = 0; i < _activeSequences.Count; i++)
                _activeSequences[i].UpdateEffectSequence();
        }

        public void Reset()
        {
            // foreach (var activeSequence in _activeSequences)
            //     activeSequence.Dispose();
            _activeSequences.Clear();
        }

        #endregion

        #region PrivateMethod

        private void PlaySequence(EffectSequenceConfig effectSequenceConfig,Action onComplete = null)
        {
            if (effectSequenceConfig.IsInterruptable)
                RemoveEffectSequence(effectSequenceConfig.ID);

            EffectSequence effectSequence = PoolManager.VisualSystemPool.GetEffectSequence(effectSequenceConfig);
            
            effectSequence.Init(_entityVisualComponent,effectSequenceConfig,onComplete);      
            
            effectSequence.OnDispose += RemoveEffectSequence;
            _activeSequences.Add(effectSequence);
        }
        
        private void RemoveEffectSequence(EffectSequence effectSequence)
        {
            if (_activeSequences.Count == 0)
                return;
            
            // if (effectSequence.IsInterruptable && effectSequence.IsActive)
            //     effectSequence.ResetSequence();

            _activeSequences.Remove(effectSequence);
            effectSequence.OnDispose -= RemoveEffectSequence;
            effectSequence.Dispose();
        }
        
        //not in use
        private bool CanPlaySequence(EffectSequenceConfig sequenceConfig,out int interrupterSequenceIndex)
        {
            for (var i = 0; i < _activeSequences.Count; i++)
            {
                if (_activeSequences[i].ID != sequenceConfig.ID) 
                    continue;

                if (_activeSequences[i].IsInterruptable)
                {
                    interrupterSequenceIndex = i;
                    return true;
                }

                interrupterSequenceIndex = -1;
                return false;
            }

            interrupterSequenceIndex = -1;
            return true;
        }

        #endregion
        
    }
}