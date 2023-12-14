using System;
using System.Collections;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.GameplayLogic.VisualSystem.EffectType
{
    [Serializable]
    public class WindEffectHandler : EffectTransitionLerp<WindParticleAnimationType>, IInitialization<ParticleSystem[]>
    {
        private ParticleSystem[] _windParticles;
        
        public bool IsInitialization { get; }
        public void Init(ParticleSystem[] windParticles)
        {
            _windParticles = windParticles;
        }

        protected override void SetValue(WindParticleAnimationType type, float value)
        {
            switch (type)
            {
                case WindParticleAnimationType.SimulationSpeed:
                    foreach (var particle in _windParticles)
                    {
                        var main = particle.main;
                        main.simulationSpeed = value;
                    }
                    break;
            }
        }
    }
    public enum WindParticleAnimationType
    {
        SimulationSpeed,
    }
}