using System;
using Tzipory.GameplayLogic.EntitySystem.Enemies;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.GameplayLogic.UIElements;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class ProximityRingHandler : MonoBehaviour, ITargetableReciever
    {
        public event Action<int> OnShadowEnter;
        public event Action<int> OnShadowExit;
        public event Action<int,Shaman> OnShamanEnter;
        public event Action<int,Shaman> OnShamanExit;
        public event Action<int,Enemy> OnEnemyEnter;
        public event Action<int,Enemy> OnEnemyExit;
        [HideInInspector]public int Id { get; private set; }
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ColliderTargetingArea _colliderTargetingArea;

        
        
        public void Init(int id)
        {
            _colliderTargetingArea.Init(this);
            Id = id;
        }
        public void Init(int id, float range, Color color)
        {
            _colliderTargetingArea.Init(this);
            Id = id;
            Scale(range);
            ChangeColor(color);
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
            _spriteRenderer.color = color;
        }

        public void RecieveCollision(Collider2D other, IOType ioType)
        {
            if (TotemPanelUIManager.TotemSelected) return;
            if (other.gameObject.CompareTag("ShadowShaman"))
            {
                if (ioType == IOType.In) OnShadowEnter?.Invoke(Id);
                else if (ioType == IOType.Out) OnShadowExit?.Invoke(Id);
            }
        }

        public void RecieveTargetableEntry(IEntityTargetAbleComponent targetable)
        {
            switch (targetable)
            {
                case Shaman shaman:
                    OnShamanEnter?.Invoke(Id,shaman);
                    break;
                case Enemy enemy:
                    OnEnemyEnter?.Invoke(Id,enemy);
                    break;
            }
        }

        public void RecieveTargetableExit(IEntityTargetAbleComponent targetable)
        {
            switch (targetable)
            {
                case Shaman shaman:
                    OnShamanExit?.Invoke(Id,shaman);
                    break;
                case Enemy enemy:
                    OnEnemyExit?.Invoke(Id,enemy);
                    break;
            }
        }
    }
}