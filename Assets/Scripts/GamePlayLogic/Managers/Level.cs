using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.SerializeData.LevalSerializeData
{
    public class Level : MonoBehaviour
    {
        //[SerializeField] private 
        [SerializeField] private List<WaveSpawner> _waveSpawners;

        private readonly List<Color> _spawnerColors = new()
        {
            Color.red,
            Color.gray,
            Color.green,
            Color.blue,
            Color.magenta,
            Color.cyan,
            Color.yellow
        };

        public IEnumerable<WaveSpawner> WaveSpawners => _waveSpawners;

        public int NumberOfWaveSpawners => _waveSpawners.Count;

        [Button("Rest waveSpawnerList")]
        private void RestWaveSpawnerList()
        {
            _waveSpawners = new List<WaveSpawner>();
            OnValidate();
        }

        private void OnValidate()
        {
            _waveSpawners ??= new List<WaveSpawner>();

            for (int i = 0; i < _waveSpawners.Count; i++)
                _waveSpawners[i].SetColor(_spawnerColors[i]);
        }
    }
}