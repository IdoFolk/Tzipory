using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tzipory.GameplayLogic.EntitySystem.PowerStructures;
using Tzipory.Systems.WaveSystem;
using UnityEngine;

namespace Tzipory.SerializeData.PlayerData.Party.Entity
{
    public class LevelHandler : MonoBehaviour
    {
        public static Vector3 FakeForward { get; private set;}

        /// <summary>
        /// Basically, the Map's resolution
        /// </summary>
        public Bounds MapBounds {
            get
            {
                if (_bgRenderer is null)
                {
                    return default;
                }

                return _bgRenderer.bounds;
            }
        }

        public static Vector2 MapStartWordPosition { get; private set; }

        public Vector2 CameraBorder => _cameraBorders;
        public float CameraMaxZoom => _cameraMaxZoom;

        public Vector2 CameraStartPosition => _cameraStartPosition;
        public float CameraStartZoom => _cameraStartZoom;

        public bool OverrideCameraStartPositionAndZoom => _overrideCameraStartPositionAndZoom;

        /// <summary>
        /// The renderer for the map/floor
        /// </summary>
        [SerializeField] SpriteRenderer _bgRenderer; 
        [SerializeField,TabGroup("Map config")] private Vector3 _fakeForward;
        [SerializeField,TabGroup("Map config")] private MapStartPosition _mapStartPosition;
        [Header("Camera setting")]
        [SerializeField,TabGroup("Camera config")] private Vector2 _cameraBorders;
        [SerializeField,TabGroup("Camera config")] private float _cameraMaxZoom;
        [SerializeField,TabGroup("Camera config")] private bool _overrideCameraStartPositionAndZoom;
        [SerializeField,TabGroup("Camera config"),ShowIf(nameof(_overrideCameraStartPositionAndZoom))] private Vector2 _cameraStartPosition;
        [SerializeField,TabGroup("Camera config"),ShowIf(nameof(_overrideCameraStartPositionAndZoom))] private float _cameraStartZoom;
        [SerializeField,TabGroup("Camera config")] private bool _enableGizmos = true;
        [Header("Serialized Components")]
        [SerializeField,OnCollectionChanged(nameof(GetWaveSpawners))] private List<WaveSpawner> _waveSpawnersSerialize;
        [SerializeField] private List<PowerStructure> _powerStructuresSerialize;
        [SerializeField] private ParticleSystem[] _particleSystemsSerialize;
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

        public static ParticleSystem[] ParticleSystems { get; private set; }

        public IEnumerable<WaveSpawner> WaveSpawners => _waveSpawners;
        public IEnumerable<WaveSpawner> WaveSpawnersSerialize => _waveSpawnersSerialize;
        
        public int NumberOfWaveSpawners => _waveSpawners.Count;
        
        private void Awake()
        {
            FakeForward = _fakeForward.normalized;
            MapStartWordPosition = GetMapStartPosition();
            
            ParticleSystems = _particleSystemsSerialize;

            foreach (var powerStructure in _powerStructuresSerialize)
            {
                powerStructure.Init();
            }
            AddWaveSpawners(_waveSpawnersSerialize);
        }

        [Button("Update Serialized Entities")]
        private void UpdateSerializedEntities()
        {
            PowerStructure[] powerStructures = GetComponentsInChildren<PowerStructure>();
            WaveSpawner[] waveSpawners = GetComponentsInChildren<WaveSpawner>();
            
            foreach (var powerStructure in powerStructures)
            {
                _powerStructuresSerialize ??= new List<PowerStructure>();
            
                if (_powerStructuresSerialize.Contains(powerStructure))
                    continue;
                _powerStructuresSerialize.Add(powerStructure);
            }

            foreach (var waveSpawner in waveSpawners)
            {
                _waveSpawnersSerialize ??= new List<WaveSpawner>();
            
                if (_waveSpawnersSerialize.Contains(waveSpawner))
                    continue;
                _waveSpawnersSerialize.Add(waveSpawner);
            }
        }
        
        private static void AddWaveSpawners(List<WaveSpawner> waveSpawners)
        {
            foreach (var waveSpawner in waveSpawners)
            {
                _waveSpawners ??= new List<WaveSpawner>();
            
                if (_waveSpawners.Contains(waveSpawner))
                    return;
                _waveSpawners.Add(waveSpawner);
            }
        }

        private void OnDestroy()
        {
            _waveSpawners.Clear();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Vector3.zero, _fakeForward.normalized * 5);
            
            //camera borders
            if (_enableGizmos)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(Vector3.zero, _cameraBorders * 2);
            }
            
        }
        
        [Button("refresh")]
        private void GetWaveSpawners()
        {
            for (int i = 0; i < _waveSpawnersSerialize.Count; i++)
            {
                _waveSpawnersSerialize[i].SetColor(_spawnerColors[i]);
                _waveSpawnersSerialize[i].SetId(i);
            }
        }

        private Vector2 GetMapStartPosition()
        {
            float mapX;
            float mapY;

            switch (_mapStartPosition)
            {
                case MapStartPosition.TopLeft:
                    mapX = MapBounds.center.x - MapBounds.extents.x;
                    mapY = MapBounds.center.y + MapBounds.extents.y;
                    break;
                case MapStartPosition.TopRight:
                    mapX = MapBounds.center.x + MapBounds.extents.x;
                    mapY = MapBounds.center.y + MapBounds.extents.y;
                    break;
                case MapStartPosition.BottomLeft:
                    mapX = MapBounds.center.x - MapBounds.extents.x;
                    mapY = MapBounds.center.y - MapBounds.extents.y;
                    break;
                case MapStartPosition.BottomRight:
                    mapX = MapBounds.center.x + MapBounds.extents.x;
                    mapY = MapBounds.center.y - MapBounds.extents.y;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return new Vector2(mapX, mapY);
        }
    }
}

public enum MapStartPosition
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight,
}
