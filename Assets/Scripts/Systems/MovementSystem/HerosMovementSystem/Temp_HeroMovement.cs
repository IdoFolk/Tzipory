using System;
using ProjectDawn.Navigation.Hybrid;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.GameplayLogic.EntitySystem.Totems;
using Tzipory.GameplayLogic.UIElements;
using Tzipory.Helpers.Consts;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.EntityComponents;
using UnityEngine;

namespace Tzipory.Systems.MovementSystem.HerosMovementSystem
{
    public class Temp_HeroMovement : MonoBehaviour
    {
        [SerializeField] private AgentAuthoring _agentAuthoring;
        [SerializeField] private TEMP_BasicMoveComponent _moveComponent;
        [SerializeField] Shaman _shaman;

        public event Action<Temp_HeroMovement> TotemPlaced;

        public Shaman Shaman => _shaman;

        public bool IsMoveing => _moveComponent.IsMoveing;

        private void Start()
        {
            _moveComponent.Init(_shaman.StatHandler.GetStat(Constant.StatsId.MovementSpeed));
        }

        public void SetTarget(Vector3 pos)
        {
            if (TotemPanelUIManager.TotemSelected.TryGetValue(_shaman.EntityInstanceID, out var value))
            {
                if (value)
                {
                    _moveComponent.SetDestination(new Vector3(pos.x, pos.y - 1, 0), MoveType.Free, PlaceTotem);
                    return;
                }
            }

            _moveComponent.SetDestination(pos, MoveType.Free); //MoveType is not really used at all
        }

        public void SelectHero()
        {
            Sprite shadowSprite = _shaman.SpriteRenderer.sprite;
            float targetRange = _shaman.TargetingRange.CurrentValue;
            if (TotemPanelUIManager.TotemSelected.TryGetValue(_shaman.EntityInstanceID, out var value))
            {
                if (value)
                {
                    shadowSprite = _shaman.TotemConfig.TotemSprite;
                    targetRange = _shaman.TotemConfig.Range;
                }
            }

            TempHeroMovementManager.Instance.SelectTarget(this, shadowSprite, targetRange); //temp?
        }

        private void PlaceTotem(Vector3 pos)
        {
            TotemManager.Instance.PlaceTotem(new Vector3(pos.x, pos.y + 1, 0), _shaman);
            TotemPlaced?.Invoke(this);
        }
    }
}