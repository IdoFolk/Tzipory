using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.SerializeData.LevalSerializeData
{
    public class Level : MonoBehaviour
    {
        public static Vector3 FakeForward;
        public static Vector2 MapSize;


        /// <summary>
        /// Basically, the Map's resolution
        /// </summary>
        [SerializeField] Vector2 _mapSize; //this could be taken from the map's actual texture if we want an easier life TBD
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
            MapSize = _mapSize;
        }

        public static void AddWaveSpawner(WaveSpawner waveSpawner)
        {
            _waveSpawners ??= new List<WaveSpawner>();
            
            if (_waveSpawners.Contains(waveSpawner))
                return;
            _waveSpawners.Add(waveSpawner);
        }

        private void OnDestroy()
        {
            _waveSpawners.Clear();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, _fakeForward.normalized * 5);
        }
        
        [Button("refrec")]
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