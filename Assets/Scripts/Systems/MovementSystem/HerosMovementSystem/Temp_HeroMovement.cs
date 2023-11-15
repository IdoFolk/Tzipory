using System;
using ProjectDawn.Navigation.Hybrid;
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
        [SerializeField] GameplayLogic.EntitySystem.Shamans.Shaman _shaman;

        public event Action TotemPlaced;

        public bool IsMoveing => _moveComponent.IsMoveing;
        
        private void Start()
        {
            _moveComponent.Init(_shaman.StatHandler.GetStat(Constant.StatsId.MovementSpeed));
        }
        public void SetTarget(Vector3 pos)
        {
            if (TotemPanelUIManager.TotemSelected)
                _moveComponent.SetDestination(new Vector3(pos.x,pos.y - 1,0), MoveType.Free,PlaceTotem);
            else
                _moveComponent.SetDestination(pos, MoveType.Free); //MoveType is not really used at all
        }

        public void SelectHero()
        {
            //TempHeroMovementManager.Instance.SelectTarget(this);
            Sprite shadowSprite;
            float targetRange;
            if (TotemPanelUIManager.TotemSelected)
            {
                shadowSprite = _shaman.TotemConfig.TotemSprite;
                targetRange = _shaman.TotemConfig.Range;
            }
            else
            {
                shadowSprite = _shaman.SpriteRenderer.sprite;
                targetRange = _shaman.TargetingRange.CurrentValue;
            }
            TempHeroMovementManager.Instance.SelectTarget(this,shadowSprite, targetRange); //temp?
        }

        private void PlaceTotem(Vector3 pos)
        {
            TotemManager.Instance.PlaceTotem(new Vector3(pos.x,pos.y + 1,0),_shaman);
            TotemPlaced?.Invoke();
        }
    }
}