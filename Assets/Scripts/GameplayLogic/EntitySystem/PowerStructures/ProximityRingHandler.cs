using System;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class ProximityRingHandler : MonoBehaviour
    {
        [HideInInspector]public int Id { get; private set; }
        public bool ActiveRing => _activeRing;
        public ColliderTargetingArea ColliderTargetingArea => _colliderTargetingArea;
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ColliderTargetingArea _colliderTargetingArea;

        private float _spriteAlpha;
        private bool _activeRing;
        
        
        public void Init(int id, ITargetableReciever reciever, float alpha)
        {
            _colliderTargetingArea.Init(reciever);
            _spriteAlpha = alpha;
            Id = id;
        }

        public void Scale(float range)
        {
            transform.localScale = new Vector3(range, range, transform.localScale.z);
        }

        public void ToggleSprite(bool state)
        {
            _spriteRenderer.enabled = state;
        }

        public void ChangeColor(Color color)
        {
            color.a = _spriteAlpha;
            _spriteRenderer.color = color;
        }

        public void ActivateRing(bool state)
        {
            _activeRing = state;
        }
    }
}