using System;
using Tzipory.GameplayLogic.EntitySystem.Shamans;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.TargetingSystem;
using Tzipory.Systems.UISystem;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.PowerStructures
{
    public class ProximityRingHandler : MonoBehaviour, ITargetableReciever
    {
        public event Action<int,Shaman,Shadow> OnShadowEnter;
        public event Action<int,Shaman,Shadow> OnShadowExit;
        public event Action<int,Shaman> OnShamanEnter;
        public event Action<int,Shaman> OnShamanExit;
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
                        OnShadowEnter?.Invoke(Id,shadow.Shaman,shadow);
                }

                if (ioType == IOType.Out)
                {
                    if (other.gameObject.TryGetComponent<Shadow>(out var shadow))
                        OnShadowExit?.Invoke(Id,shadow.Shaman,shadow);
                }
            }
            if (other.gameObject.CompareTag("Shaman"))
            {
                if (ioType == IOType.In)
                {
                    if (other.gameObject.transform.parent.TryGetComponent<Shaman>(out var shaman))
                        OnShamanEnter?.Invoke(Id,shaman);
                }

                if (ioType == IOType.Out)
                {
                    if (other.gameObject.transform.parent.TryGetComponent<Shaman>(out var shaman))
                        OnShamanExit?.Invoke(Id,shaman);
                }
            }
        }

        public void RecieveTargetableEntry(IEntityTargetAbleComponent targetable)
        {
            
        }

        public void RecieveTargetableExit(IEntityTargetAbleComponent targetable)
        {
            
        }
    }
}