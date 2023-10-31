using System;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.Systems.TargetingSystem
{
    public class ColliderTargetingArea : MonoBehaviour
    {
        public bool IsColliding => _isColliding;

        [SerializeField] private bool _testing = false;
        
        private ITargetableReciever _reciever;
        private bool _isColliding;
        
        [Obsolete]
        
        public void Init(ITargetableReciever  reciever)
        {
            _reciever = reciever;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_testing)
                Debug.Log($"On target enter {other.name} from {gameObject.name}");
            
            _reciever.RecieveCollision(other, IOType.In);
            if (other.gameObject.CompareTag("ShadowShaman"))
                _isColliding = true;
            
            if (!other.TryGetComponent<IEntityTargetAbleComponent>(out var targetAbleComponent)) return;
            _reciever.RecieveTargetableEntry(targetAbleComponent);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_testing)
                Debug.Log($"On target exit {other.name} from {gameObject.name}");
            
            _reciever.RecieveCollision(other, IOType.Out);
            if (other.gameObject.CompareTag("ShadowShaman"))
                _isColliding = false;

            if (!other.TryGetComponent<IEntityTargetAbleComponent>(out var targetAbleComponent)) return;
            _reciever.RecieveTargetableExit(targetAbleComponent);
        }

    }

    public interface ITargetableReciever
    {
        void RecieveCollision(Collider2D other, IOType ioType);
        void RecieveTargetableEntry(IEntityTargetAbleComponent targetable);
        void RecieveTargetableExit(IEntityTargetAbleComponent targetable);
    }
}