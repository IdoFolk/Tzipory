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
        public event Action<int,Shaman> OnShadowEnter;
        public event Action<int,Shaman> OnShadowExit;
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
                    OnShadowEnter?.Invoke(Id,other.gameObject.GetComponent<Shadow>().Shaman);
                }

                if (ioType == IOType.Out)
                {
                    OnShadowExit?.Invoke(Id,other.gameObject.GetComponent<Shadow>().Shaman);
                }
            }
        }

        public void RecieveTargetableEntry(IEntityTargetAbleComponent targetable)
        {
            if (targetable is not Shaman shaman) return;
            
            OnShamanEnter?.Invoke(Id,shaman);
        }

        public void RecieveTargetableExit(IEntityTargetAbleComponent targetable)
        {
            if (targetable is not Shaman shaman) return;

            OnShamanExit?.Invoke(Id,shaman);
        }
    }
}