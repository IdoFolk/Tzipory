using System.Collections.Generic;
using SerializeData.LevalSerializeData.PartySerializeData;
using Shamans;
using Tzipory.EntitySystem.EntityConfigSystem;
using UnityEngine;

namespace GameplayeLogic.Managers
{
    public class PartyManager
    {
        private readonly Shaman  _shamanPrefab;
        private readonly Transform _partyParent;
        
        public IEnumerable<Shaman> Party { get; private set; }
        
        public PartyManager(PartySerializeData partySerializeData)
        {
            Party = CreateParty(partySerializeData.EntityConfigs);

            _shamanPrefab = partySerializeData.ShamanPrefab;
            _partyParent = partySerializeData.PartyParent;
        }

        private IEnumerable<Shaman> CreateParty(IEnumerable<ShamanConfig> party)
        {
            foreach (var entityConfig in party)
            {
                var shaman = Object.Instantiate(_shamanPrefab, _partyParent);
                shaman.Init(entityConfig);
                yield return shaman;
            }
        }
    }
}