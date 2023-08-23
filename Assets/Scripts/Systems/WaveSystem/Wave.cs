using System;
using System.Collections.Generic;
using System.Linq;
using Tzipory.SerializeData.LevalSerializeData;
using Tzipory.Tools.Interface;

namespace Tzipory.WaveSystem
{
    public class Wave : IInitialization
    {
        private readonly WaveSpawner[] _waveSpawners;

        private readonly WaveConfig _data;

        public bool IsStarted { get; private set; }
        
        public float CompletionPercentage => throw new NotImplementedException();
        public bool IsDone => _waveSpawners.All(waveSpawner => waveSpawner.IsDone) && IsStarted;

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
        
        public bool IsInitialization { get; private set; }

        public Wave(IEnumerable<WaveSpawner> waveSpawners,WaveConfig waveConfig)
        {
            _data = waveConfig;
            _waveSpawners  = waveSpawners.ToArray();
            IsInitialization = false;
        }
        
        public void Init()
        {
            foreach (var waveSpawner in _waveSpawners)
            {
                foreach (var waveSpawnerSerializeData in _data.WaveSpawnerConfig)
                {
                    if (waveSpawner.ID == waveSpawnerSerializeData.ID)
                        waveSpawner.Init(waveSpawnerSerializeData);
                }
            }

            IsInitialization = true;
        }

        public IEnumerable<WaveSpawner> GetActiveWaveSpawners() =>
            _waveSpawners.Where(waveSpawner => waveSpawner.IsActiveThisWave);

        public void StartWave()
        {
            foreach (var waveSpawner in _waveSpawners)
                waveSpawner.StartSpawning();

            IsStarted = true;
        }
        
        public void EndWave()
        {
        }
    }
}