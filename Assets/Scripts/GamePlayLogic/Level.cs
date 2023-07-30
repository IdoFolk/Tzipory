using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tzipory.SerializeData.LevalSerializeData
{
    public class Level : MonoBehaviour
    {
        public static Vector3 FakeForward { get; private set;}
        /// <summary>
        /// Basically, the Map's resolution
        /// </summary>
        public static Vector2 MapSize { get; private set; }
        
        /// <summary>
        /// The renderer for the map/floor
        /// </summary>
        [SerializeField] SpriteRenderer _bgRenderer; 
        [SerializeField] private Vector3 _fakeForward;
        [SerializeField] private Transform _entityParent;
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

        public Transform EntityParent => _entityParent;

        private void Awake()
        {
            FakeForward = _fakeForward;
            MapSize = new Vector2(_bgRenderer.sprite.texture.width, _bgRenderer.sprite.texture.height);
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