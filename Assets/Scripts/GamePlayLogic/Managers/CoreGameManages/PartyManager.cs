using System;
using System.Collections.Generic;
using System.Linq;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.GamePlayLogic.ObjectPools;
using Tzipory.SerializeData.PlayerData.Party;
using Tzipory.SerializeData.PlayerData.Party.Entity;
using Tzipory.Systems.FactorySystem.GameObjectFactory;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Tzipory.GameplayLogic.Managers.CoreGameManagers
{
    public class PartyManager : IDisposable
    {
        private readonly UnitEntity  _shamanPrefab;
        private readonly Transform _partyParent;
        private readonly Dictionary<Vector3, bool> _partySpawnPoints;
        private readonly PartySerializeData _partySerializeData;
        private const string SHAMAN_PREFAB_PATH = "Prefabs/Entities/Shaman/BaseShamanEntity";

        public UnitEntity[] Party;//Temp fix

        public PartyManager(PartySerializeData partySerializeData,Transform partyParent)
        {
            _partySerializeData  = partySerializeData;
            _partySpawnPoints = new Dictionary<Vector3,bool>();
            _shamanPrefab = Resources.Load<UnitEntity>(SHAMAN_PREFAB_PATH);
            _partyParent = partyParent;
        }

        public void SpawnShaman()
        {
            Party = CreateParty(_partySerializeData.ShamanSerializeDatas).ToArray();
        }
        
        public void AddSpawnPoint(Vector3 spawnPoint)=>
            _partySpawnPoints.Add(spawnPoint, false);

        public UnitEntity GetShaman(int shamanId)
        {
            foreach (var shaman in Party)
            {
                if (shaman.EntityInstanceID == shamanId) return shaman;
            }

            return null;
        }
        private IEnumerable<UnitEntity> CreateParty(IEnumerable<ShamanSerializeData> party)
        {
            foreach (var shamanSerializeData in party)
            {
                var shaman = PoolManager.ShamanPool.GetObject();
                shaman.transform.position = GetSpawnPoint();
                shaman.transform.SetParent(_partyParent);
                shaman.Init(shamanSerializeData);
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