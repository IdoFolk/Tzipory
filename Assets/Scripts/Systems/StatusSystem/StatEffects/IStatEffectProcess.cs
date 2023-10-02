using System;
using Tzipory.Systems.StatusSystem;

namespace Tzipory.EntitySystem.StatusSystem
{
    public interface IStatEffectProcess : IDisposable
    {
        public event Action<int> OnDispose;
        public string StatProcessName { get; }
        public int StatProcessInstanceID { get; }
        public int StatProcessPriority { get; }
        public Stat StatToEffect { get; }
        public StatModifier StatModifier { get; }
        public StatEffectType StatEffectType { get; }
        public bool ProcessEffect(ref float statValue);
    }
    
    public enum StatEffectType
    {
        OverTime,
        Instant,
        Interval,
        Process
    }
}