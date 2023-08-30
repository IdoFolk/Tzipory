using System;
using Tools.Enums;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

namespace Systems.TargetingSystem
{
    public class ColliderTargetingArea : MonoBehaviour
    {
        [SerializeField] private bool _testing = false;
        
        private ITargetableReciever _reciever;
        
        [Obsolete]
        public void Init()
        {
            _reciever = GetComponentInParent(typeof(ITargetableReciever)) as ITargetableReciever;

            if (_reciever == null)
                Debug.LogError($"{transform.parent.name} did not get a <color=#ff0000>ITargetableReciever:</color>");
        }
        
        public void Init(ITargetableReciever  reciever)
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