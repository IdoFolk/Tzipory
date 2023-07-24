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

        private List<EffectSequence> _activeSequences;

        private IEntityVisualComponent _entityVisualComponent;

        public EffectSequenceHandler(IEntityVisualComponent visualComponent,IEnumerable<EffectSequenceConfig> sequencesDatas)
        {
            _sequencesDictionary = new();
            _activeSequences = new();
            
            _entityVisualComponent = visualComponent;

            foreach (var sequenceData in sequencesDatas)
                _sequencesDictionary.Add(sequenceData.ID, sequenceData);
        }

        private void PlaySequence(EffectSequenceConfig effectSequenceConfig)
        {
            if (effectSequenceConfig.IsInterruptable)
                RemoveEffectSequence(effectSequenceConfig.ID);

            EffectSequence effectSequence = PoolManager.VisualSystemPool.GetEffectSequence(effectSequenceConfig);
            
            effectSequence.Init(_entityVisualComponent,effectSequenceConfig);      
            
            effectSequence.StartEffectSequence();
            
            effectSequence.OnEffectSequenceComplete += RemoveEffectSequence;
            _activeSequences.Add(effectSequence);
        }
        
        
        private void RemoveEffectSequence(EffectSequence effectSequence)
        {
            if (_activeSequences.Count == 0)
                return;

            // if (!_activeSequences.Contains(effectSequence)) //will make to loop over the _activeSequences list may make some problems 
            // {
            //     Debug.LogWarning("Try remove a effectSequence that not exists");
            //     return;
            // }


            if (effectSequence.IsInterruptable && effectSequence.IsActive)
                effectSequence.ResetSequence();

            effectSequence.OnEffectSequenceComplete -= RemoveEffectSequence;
            _activeSequences.Remove(effectSequence);
        }

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
        
        public void PlaySequenceById(int id)
        {
            if (!_sequencesDictionary.TryGetValue(id, out var effectSequenceData))
            {
                Debug.LogWarning($"Sequence with id {id} not found");
                return;
            }
            
            PlaySequence(effectSequenceData);
        }
        
        public void PlaySequenceByData(EffectSequenceConfig effectSequenceConfig)
        {
            if (effectSequenceConfig.EffectActionContainers.Count == 0)
                return;
            
            PlaySequence(effectSequenceConfig);
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
                    return;
                }
            }
            
            Debug.LogWarning($"Try remove a effectSequence that not exists effect ID : {effectSequenceId}");
        }
        
        public void UpdateEffectHandler()
        {
            for (int i = 0; i < _activeSequences.Count; i++)
            {
                _activeSequences[i].UpdateEffectSequence();
            }
        }
    }
}