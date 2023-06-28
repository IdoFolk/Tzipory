﻿using ProjectDawn.Navigation.Hybrid;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

namespace MovementSystem.HerosMovementSystem
{
    public class Temp_HeroMovement : MonoBehaviour
    {
        [SerializeField] private AgentAuthoring _agentAuthoring;
        [SerializeField] private BasicMoveComponent _moveComponent;
        [SerializeField] Shamans.Shaman _shaman;

        public bool IsMoveing => _moveComponent.IsMoveing;
        
        private void Start()
        {
            _moveComponent.Init(_shaman.MoveSpeed);
        }
        public void SetTarget(Vector3 pos)
        {
            _moveComponent.SetDestination(pos, MoveType.Free); //MoveType is not really used at all
        }

        public void SelectHero()
        {
            TempHeroMovementManager.Instance.SelectTarget(this);
            Debug.Log($"{gameObject.name} has Selected");
        }
    }
}