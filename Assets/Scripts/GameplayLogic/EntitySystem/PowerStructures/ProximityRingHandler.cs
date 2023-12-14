using System;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class ProximityRingHandler : MonoBehaviour, ITargetableCollisionReciever
    {
        public event Action<int,ITargetAbleEntity> OnShadowEnter;
        public event Action<int,ITargetAbleEntity> OnShadowExit;
        public event Action<int,ITargetAbleEntity> OnShamanEnter;
        public event Action<int,ITargetAbleEntity> OnShamanExit;
        [HideInInspector]public int Id { get; private set; }
        
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private ColliderTargetingArea _colliderTargetingArea;

        private float _spriteAlpha;
        
        
        public void Init(int id, float alpha)
        {
            _colliderTargetingArea.Init(this);
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

        public void RecieveCollision(Collider2D other, IOType ioType)
        {
            if (other.gameObject.CompareTag("ShadowShaman"))
            {
                if (ioType == IOType.In)
                {
                    if (other.gameObject.TryGetComponent<Shadow>(out var shadow))
                        OnShadowEnter?.Invoke(Id,shadow.Shaman);
                }

                if (ioType == IOType.Out)
                {
                    if (other.gameObject.TryGetComponent<Shadow>(out var shadow))
                        OnShadowExit?.Invoke(Id,shadow.Shaman);
                }
            }
            if (other.gameObject.CompareTag("Shaman"))
            {
                if (ioType == IOType.In)
                {
                    if (other.gameObject.transform.parent.TryGetComponent<UnitEntity>(out var unitEntity))
                    {
                        if (unitEntity.EntityType == EntityType.Hero)
                            OnShamanEnter?.Invoke(Id,unitEntity);
                    }
                }

                if (ioType == IOType.Out)
                {
                    if (other.gameObject.transform.parent.TryGetComponent<UnitEntity>(out var unitEntity))
                    {
                        if (unitEntity.EntityType == EntityType.Hero)
                            OnShamanExit?.Invoke(Id,unitEntity);
                    }
                }
            }
        }
    }
}