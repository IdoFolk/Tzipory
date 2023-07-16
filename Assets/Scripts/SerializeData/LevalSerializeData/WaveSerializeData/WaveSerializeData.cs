using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.SerializeData.LevalSerializeData
{
    [System.Serializable]
    public class WaveSerializeData
    {
        [Title("$_name",bold:true,titleAlignment:TitleAlignments.Centered)] 
        [SerializeField,ReadOnly,PropertyOrder(-2)] private string _name;
        [SerializeField,ListDrawerSettings(HideAddButton = true,HideRemoveButton = true),PropertyOrder(1)] private List<WaveSpawnerSerializeData> _waveSpawnerSerializeDatas;

        [ShowInInspector, ReadOnly,PropertyOrder(-1)]
        public float TotalWaveTime
        {
            get
            {
                float totalWaveTime = 0;

                foreach (var spawnerSerializeData in _waveSpawnerSerializeDatas)
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
        
        private LevelSerializeData  _levelSerializeData;
        public List<WaveSpawnerSerializeData> WaveSpawnerSerializeDatas => _waveSpawnerSerializeDatas;

        public WaveSerializeData(IEnumerable<WaveSpawner> waveSpawners,LevelSerializeData levelSerializeData)
        {
            _levelSerializeData = levelSerializeData;
            _waveSpawnerSerializeDatas = new List<WaveSpawnerSerializeData>();

            foreach (var waveSpawner in waveSpawners)
            {
                _waveSpawnerSerializeDatas.Add(new WaveSpawnerSerializeData(waveSpawner));
            }
        }
        
        public void SetWaveSpawners(IEnumerable<WaveSpawner> waveSpawners)
        {
            if (_waveSpawnerSerializeDatas == null)
                _waveSpawnerSerializeDatas = new List<WaveSpawnerSerializeData>();

            foreach (var waveSpawner in waveSpawners)
            {
                _waveSpawnerSerializeDatas.Add(new WaveSpawnerSerializeData(waveSpawner));
            }
        }
        
        public void SetName(string  name)=>
            _name = name;

        public void AddWaveSpawner(WaveSpawner waveSpawner)
        {
            _waveSpawnerSerializeDatas.Add(new WaveSpawnerSerializeData(waveSpawner));
        }
        
        public void RemoveWaveSpawner(WaveSpawner waveSpawner)
        {
        }

        [Button("Delete wave")]
        public void DeleteWave()=>
            _levelSerializeData.RemoveWave(this);

        public void OnValidate(float startTime)
        {
            foreach (var spawnerSerializeData in _waveSpawnerSerializeDatas)
            {
                spawnerSerializeData.OnValidate(startTime);  
            }
        }
    }
}