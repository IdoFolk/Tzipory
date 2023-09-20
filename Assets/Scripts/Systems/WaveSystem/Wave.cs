using System;
using System.Collections.Generic;
using System.Linq;
using Tzipory.SerializeData.PlayerData.PartySerializeData.EntitySerializeData;
using Tzipory.Tools.Interface;

namespace Tzipory.Systems.WaveSystem
{
    public class Wave : IInitialization
    {
        private readonly WaveSpawner[] _waveSpawners;

        private readonly WaveConfig _data;

        public bool IsActive { get; private set; }
        
        public bool IsComplete  { get; private set; }
        public float CompletionPercentage => throw new NotImplementedException();
        public bool IsAllWaveSpawnersDone => _waveSpawners.All(waveSpawner => waveSpawner.IsDone);

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

            IsComplete = false;
            IsInitialization = true;
        }

        public IEnumerable<WaveSpawner> GetActiveWaveSpawners() =>
            _waveSpawners.Where(waveSpawner => waveSpawner.IsActiveThisWave);

        public void StartWave()
        {
            foreach (var waveSpawner in _waveSpawners)
                waveSpawner.StartSpawning();

            IsActive = true;
        }
        
        public void EndWave()
        {
            IsComplete = true;
            IsActive = false;
        }
    }
}