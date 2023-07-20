using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.EntitySystem;
using UnityEngine;

namespace Tzipory.SerializeData.LevalSerializeData
{
    public class Level : MonoBehaviour
    {
        public static Vector3 FakeForward;
        [SerializeField] private Vector3 _fakeForward;
        [SerializeField,OnCollectionChanged(nameof(GetWaveSpawners))] private List<WaveSpawner> _waveSpawnersSerialize;
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

        private void Awake()
        {
            FakeForward = _fakeForward;
        }

        public static void AddWaveSpawner(WaveSpawner waveSpawner)
        {
            _waveSpawners ??= new List<WaveSpawner>();
            
            if (_waveSpawners.Contains(waveSpawner))
                return;
            _waveSpawners.Add(waveSpawner);
        }

        // [Button("Refresh waveSpawnerList")]
        // private void RestWaveSpawnerList()
        // {
        //     _waveSpawnersSerialize = new List<WaveSpawner>();
        //     GetWaveSpawners();
        // }

        private void OnDestroy()
        {
            _waveSpawners.Clear();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, _fakeForward.normalized * 5);
        }

        private void GetWaveSpawners()
        {
            for (int i = 0; i < _waveSpawnersSerialize.Count; i++)
            {
                _waveSpawnersSerialize[i].SetColor(_spawnerColors[i]);
                _waveSpawnersSerialize[i].SetId(i);
            }
        }
    }
}