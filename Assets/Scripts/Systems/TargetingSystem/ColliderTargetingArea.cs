using System;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Tools.Enums;
using UnityEngine;

namespace Tzipory.Systems.TargetingSystem
{
    public class ColliderTargetingArea : MonoBehaviour
    {
        [SerializeField] private bool _testing;
        
        private ITargetableReciever _reciever;
        
        [Obsolete]
        
        public void Init(ITargetableReciever reciever)
        {
            _reciever = reciever;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_testing)
                Debug.Log($"On target enter {other.name} from {gameObject.name}");

            _reciever.RecieveCollision(other, IOType.In);
            
            if (!other.TryGetComponent<IEntityTargetAbleComponent>(out var targetAbleComponent)) return;
                 _reciever.RecieveTargetableEntry(targetAbleComponent);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_testing)
                Debug.Log($"On target exit {other.name} from {gameObject.name}");
            
            _reciever.RecieveCollision(other, IOType.Out);

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