
using System;
using System.Collections.Generic;
using Tzipory.ConfigFiles.StatusSystem;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.StatusSystem;

namespace Tzipory.Systems.Entity.EntityComponents
{
    public interface IEntityStatComponent : IEntityComponent
    {
        event Action<EffectSequenceConfig> OnStatusEffectAdded;//Temp!

        public IDisposable AddStatEffect(StatEffectConfig statEffectConfig);
        public Stat GetStat(Constant.StatsId statIdToFind);

        public Stat GetStat(int id);
        
        public IEnumerable<Stat> GetAllStats();
    }
}