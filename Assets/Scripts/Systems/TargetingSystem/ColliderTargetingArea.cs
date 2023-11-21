using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Tools.Enums;
using Tzipory.Tools.Interface;
using UnityEngine;

namespace Tzipory.Systems.TargetingSystem
{
    public class ColliderTargetingArea : MonoBehaviour
    {
        [SerializeField] private bool _testing;

        private ITargetableCollisionReciever _collisionReciever;
        private ITargetableEntryReciever _entryReciever;
        private ITargetableExitReciever _exitReciever;
        
        public void Init(ITargetableReciever reciever)
        {
            if (reciever is ITargetableCollisionReciever collisionReciever)
                _collisionReciever = collisionReciever;
            if (reciever is ITargetableEntryReciever entryReciever)
                _entryReciever = entryReciever;
            if (reciever is ITargetableExitReciever exitReciever)
                _exitReciever = exitReciever;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_testing)
                Debug.Log($"On target enter {other.name} from {gameObject.name}");

            _collisionReciever?.RecieveCollision(other, IOType.In);

            if (!other.TryGetComponent<IEntityTargetAbleComponent>(out var targetAbleComponent)) return;
            
            _entryReciever?.RecieveTargetableEntry(targetAbleComponent);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_testing)
                Debug.Log($"On target exit {other.name} from {gameObject.name}");

            _collisionReciever?.RecieveCollision(other, IOType.In);

            if (!other.TryGetComponent<IEntityTargetAbleComponent>(out var targetAbleComponent)) return;
            
            _exitReciever?.RecieveTargetableExit(targetAbleComponent);
        }
    }
}