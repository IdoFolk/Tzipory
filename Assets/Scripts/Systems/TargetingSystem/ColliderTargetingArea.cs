using Tools.Enums;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

namespace Systems.TargetingSystem
{
    public class ColliderTargetingArea : MonoBehaviour
    {
        [SerializeField] private bool _testing = false;
        
        private ITargetableReciever _reciever;

        private void Awake()
        {
            _reciever = GetComponentInParent(typeof(ITargetableReciever)) as ITargetableReciever;

            if (_reciever == null)
                Debug.LogError($"{transform.parent.name} did not get a <color=#ff0000>ITargetableReciever:</color>");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_testing)
                Debug.Log($"On target enter {other.name} from {gameObject.name}");
            
            _reciever.RecieveCollision(other, IOStatType.In);
            
            if (!other.TryGetComponent<IEntityTargetAbleComponent>(out var targetAbleComponent)) return;
                _reciever.RecieveTargetableEntry(targetAbleComponent);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (_testing)
                Debug.Log($"On target exit {other.name} from {gameObject.name}");
            
            _reciever.RecieveCollision(other, IOStatType.Out);
            
            if (!other.TryGetComponent<IEntityTargetAbleComponent>(out var targetAbleComponent)) return;
                _reciever.RecieveTargetableExit(targetAbleComponent);
        }

    }

    public interface ITargetableReciever
    {
        void RecieveCollision(Collider2D other, IOStatType ioStatType);
        void RecieveTargetableEntry(IEntityTargetAbleComponent targetable);
        void RecieveTargetableExit(IEntityTargetAbleComponent targetable);
    }
}