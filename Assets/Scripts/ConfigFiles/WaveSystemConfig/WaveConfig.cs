using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.Systems.WaveSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Tzipory.ConfigFiles.Level
{
    [System.Serializable]
    public class WaveConfig
    {
        [Title("$_name",bold:true,titleAlignment:TitleAlignments.Centered)] 
        [SerializeField,ReadOnly,PropertyOrder(-2)] private string _name;
        [SerializeField,ListDrawerSettings(HideAddButton = true,HideRemoveButton = true),PropertyOrder(1)] 
        [FormerlySerializedAs("_waveSpawnerSerializeDatas")] private List<WaveSpawnerConfig> _waveSpawnerConfig;

        [SerializeField,HideInInspector] private LevelConfig  _levelConfig;
        
        [ShowInInspector, ReadOnly,PropertyOrder(-1)]
        public float TotalWaveTime
        {
            get
            {
                float totalWaveTime = 0;

                foreach (var spawnerSerializeData in _waveSpawnerConfig)
                {
                    if (spawnerSerializeData == null)
                        continue;
                    if (spawnerSerializeData.TotalSpawnerTime > totalWaveTime)
                    {
                        totalWaveTime = spawnerSerializeData.TotalSpawnerTime;
                    }
                }

                return totalWaveTime;
            }
        }
        
        public List<WaveSpawnerConfig> WaveSpawnerConfig => _waveSpawnerConfig;

        public WaveConfig(IEnumerable<WaveSpawner> waveSpawners,LevelConfig levelConfig)
        {
            _levelConfig = levelConfig;
            _waveSpawnerConfig = new List<WaveSpawnerConfig>();

            foreach (var waveSpawner in waveSpawners)
            {
                _waveSpawnerConfig.Add(new WaveSpawnerConfig(waveSpawner));
            }
        }

        public void SetName(string  name)=>
            _name = name;

        [Button("Delete wave")]
        public void DeleteWave()=>
            _levelConfig.RemoveWave(this);

        public void OnValidate(float startTime)
        {
            foreach (var spawnerSerializeData in _waveSpawnerConfig)
            {
                spawnerSerializeData.OnValidate(startTime);  
            }
        }
    }
}