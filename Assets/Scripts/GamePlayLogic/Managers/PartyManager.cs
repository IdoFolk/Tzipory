using System;
using System.Collections.Generic;
using SerializeData.LevalSerializeData.PartySerializeData;
using Shamans;
using Tzipory.EntitySystem.EntityConfigSystem;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameplayeLogic.Managers
{
    public class PartyManager : IDisposable
    {
        private readonly Shaman  _shamanPrefab;
        private readonly Transform _partyParent;
        private readonly Dictionary<Vector3, bool> _partySpawnPoints;
        private readonly PartySerializeData _partySerializeData;
        public IEnumerable<Shaman> Party { get; private set; }
        
        public PartyManager(PartySerializeData partySerializeData)
        {
            _partySerializeData  = partySerializeData;
            
            _partySpawnPoints = new Dictionary<Vector3,bool>();
            _shamanPrefab = _partySerializeData.ShamanPrefab;
            _partyParent = _partySerializeData.PartyParent;
        }

        public void SpawnShaman()=>
            Party = CreateParty(_partySerializeData.EntityConfigs);

        public void AddSpawnPoint(Vector3 spawnPoint)=>
            _partySpawnPoints.Add(spawnPoint, false);

        private IEnumerable<Shaman> CreateParty(IEnumerable<ShamanConfig> party)
        {
            foreach (var entityConfig in party)
            {
                var shaman = Object.Instantiate(_shamanPrefab,GetSpawnPoint(),Quaternion.identity,_partyParent);
                shaman.Init(entityConfig);
                yield return shaman;
            }
        }

        private Vector3 GetSpawnPoint()
        {
            foreach (var spawnPoint in _partySpawnPoints)
            {
                if (!spawnPoint.Value) 
                {
                    _partySpawnPoints[spawnPoint.Key] = true;
                    return spawnPoint.Key;
                }
            }
            
            return Vector3.zero;
        }

        public void Dispose()
        {
        }
    }
}