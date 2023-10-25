using System;
using System.Collections.Generic;
using System.Linq;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.SerializeData.PlayerData.Party;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Tzipory.GameplayLogic.Managers.CoreGameManagers
{
    public class PartyManager : IDisposable
    {
        private readonly Shaman  _shamanPrefab;
        private readonly Transform _partyParent;
        private readonly Dictionary<Vector3, bool> _partySpawnPoints;
        private readonly PartySerializeData _partySerializeData;
        private const string SHAMAN_PREFAB_PATH = "Prefabs/Entities/Shaman/BaseShamanEntity";

        public IEnumerable<Shaman> Party { get; private set; }

        public PartyManager(PartySerializeData partySerializeData,Transform partyParent)
        {
            _partySerializeData  = partySerializeData;
            _partySpawnPoints = new Dictionary<Vector3,bool>();
            _shamanPrefab = Resources.Load<Shaman>(SHAMAN_PREFAB_PATH);
            _partyParent = partyParent;
        }

        public void SpawnShaman()=>
            Party = CreateParty(_partySerializeData.ShamansPartyDataContainers);

        public void AddSpawnPoint(Vector3 spawnPoint)=>
            _partySpawnPoints.Add(spawnPoint, false);

        private IEnumerable<Shaman> CreateParty(IEnumerable<ShamanDataContainer> party)
        {
            foreach (var shamanDataContainer in party)
            {
                var shaman = Object.Instantiate(_shamanPrefab,GetSpawnPoint(),Quaternion.identity,_partyParent);
                
                shaman.Init(shamanDataContainer.ShamanSerializeData,shamanDataContainer.UnitEntityVisualConfig);
                yield return shaman;
            }
        }

        private Vector3 GetSpawnPoint()
        {
            if (_partySpawnPoints.All(spawnPoint => spawnPoint.Value))
                return Vector3.zero;

            while (true)
            {
                var spawnPoint = _partySpawnPoints.ElementAt(Random.Range(0, _partySpawnPoints.Count));

                if (!spawnPoint.Value)
                {
                    _partySpawnPoints[spawnPoint.Key] = true;
                    return spawnPoint.Key;
                }
            }
        }

        public void Dispose()
        {
        }
    }
}