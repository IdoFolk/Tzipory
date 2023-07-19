using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.SerializeData.LevalSerializeData
{
    public class Level : MonoBehaviour
    {
        [SerializeField] private List<WaveSpawner> _waveSpawnersSerialize;
        private static List<WaveSpawner> _waveSpawners;

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
        public IEnumerable<WaveSpawner> WaveSpawnersSerialize => _waveSpawnersSerialize;

        public int NumberOfWaveSpawners => _waveSpawners.Count;

        public static void AddWaveSpawner(WaveSpawner waveSpawner)
        {
            _waveSpawners ??= new List<WaveSpawner>();
            
            if (_waveSpawners.Contains(waveSpawner))
                return;
            _waveSpawners.Add(waveSpawner);
        }

        [Button("Rest waveSpawnerList")]
        private void RestWaveSpawnerList()
        {
            _waveSpawnersSerialize = new List<WaveSpawner>();
            OnValidate();
        }

        private void OnDestroy()
        {
            _waveSpawners.Clear();
        }

        private void OnValidate()
        {
            _waveSpawnersSerialize ??= new List<WaveSpawner>();

            for (int i = 0; i < _waveSpawnersSerialize.Count; i++)
                _waveSpawnersSerialize[i].SetColor(_spawnerColors[i]);
        }
    }
}