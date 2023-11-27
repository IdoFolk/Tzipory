﻿using ProjectDawn.Navigation.Hybrid;
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
        [SerializeField] Tzipory.GameplayLogic.EntitySystem.Shamans.Shaman _shaman;

        public bool IsMoveing => _moveComponent.IsMoveing;
        
        private void Start()
        {
            _moveComponent.Init(_shaman.EntityStatComponent.GetStat(Constant.StatsId.MovementSpeed));
        }
        public void SetTarget(Vector3 pos)
        {
            _moveComponent.SetDestination(pos, MoveType.Free); //MoveType is not really used at all
        }

        public void SelectHero()
        {
            //TempHeroMovementManager.Instance.SelectTarget(this);
            TempHeroMovementManager.Instance.SelectTarget(this,_shaman.EntityVisualComponent.SpriteRenderer.sprite, _shaman.EntityTargetingComponent.TargetingRange.CurrentValue); //temp?
        }
    }
}