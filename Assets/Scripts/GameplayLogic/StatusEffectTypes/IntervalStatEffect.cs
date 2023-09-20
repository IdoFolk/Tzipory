using Helpers.Consts;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.StatusSystem;

namespace Tzipory.SerializeData.LevalSerializeData.StstusEffectTypes
{
    public class IntervalStatEffect : BaseModifyStatEffect
    {
        private float _currentDuration;
        private float _currentInterval;
        
        public override void Init(StatEffectConfig parameter, Stat statToEffectToEffect)
        {
            base.Init(parameter, statToEffectToEffect);
            
            Stats.Add((int)Constant.StatsId.Duration, new Stat("Duration", parameter.Duration, float.MaxValue, (int)Constant.StatsId.Duration));
            Stats.Add((int)Constant.StatsId.Interval, new Stat("Interval", parameter.Interval, float.MaxValue, (int)Constant.StatsId.Interval));

            _currentInterval = Stats[(int)Constant.StatsId.Interval].CurrentValue;
            _currentDuration = Stats[(int)Constant.StatsId.Duration].CurrentValue;
        }
        
        public override bool ProcessEffect(ref float statValue)
        {
            if (_currentDuration < 0)
            {
                Dispose();
                
                return false;
            }

            if (_currentInterval <= 0)
            {
                _currentInterval = Stats[(int)Constant.StatsId.Interval].CurrentValue;
                //process on stat
                StatToEffect.ProcessStatModifier(StatModifier,StatProcessName);
                return true;
            }
            
            _currentDuration -= GAME_TIME.GameDeltaTime;
            _currentInterval -= GAME_TIME.GameDeltaTime;
            
            return false;
        }
    }
}