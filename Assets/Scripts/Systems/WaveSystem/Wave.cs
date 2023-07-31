using System;
using System.Collections.Generic;
using System.Linq;
using Tzipory.SerializeData.LevalSerializeData;

namespace Tzipory.WaveSystem
{
    public class Wave : WaveComponent<WaveConfig>
    {
        private readonly WaveSpawner[] _waveSpawners;
        
        public override WaveConfig Data { get; }

        public bool IsStarted { get; private set; }
        
        public override float CompletionPercentage => throw new NotImplementedException();
        public override bool IsDone => _waveSpawners.All(waveSpawner => waveSpawner.IsDone) && IsStarted;

        public int NumberOfEnemiesInWave
        {
            get
            {
                int allEnemiesInWave = 0;

                foreach (var waveSpawner in _waveSpawners)
                    allEnemiesInWave += waveSpawner.TotalNumberOfEnemiesPreWave;
                
                return allEnemiesInWave;
            }
        }

        public Wave(IEnumerable<WaveSpawner> waveSpawners,WaveConfig waveConfig)
        {
            Data = waveConfig;
            _waveSpawners  = waveSpawners.ToArray();
        }

        public void StartWave()
        {
            foreach (var waveSpawner in _waveSpawners)
            {
                foreach (var waveSpawnerSerializeData in Data.WaveSpawnerSerializeDatas)
                {
                    if (waveSpawner.ID == waveSpawnerSerializeData.ID)
                        waveSpawner.Init(waveSpawnerSerializeData);
                }
            }

            IsStarted = true;
        }
        
        public void EndWave()
        {
        }
    }
}