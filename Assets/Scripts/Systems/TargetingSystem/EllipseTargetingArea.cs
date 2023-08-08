using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

public class EllipseTargetingArea : MonoBehaviour
{
    private ITargetableReciever _reciever;

    private void Awake()
    {
        _reciever = GetComponentInParent(typeof(ITargetableReciever)) as ITargetableReciever;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<IEntityTargetAbleComponent>(out var targetAbleComponent)) return;
        _reciever.RecieveTargetableEntry(targetAbleComponent);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<IEntityTargetAbleComponent>(out var targetAbleComponent)) return;
        _reciever.RecieveTargetableExit(targetAbleComponent);
    }

}

public interface ITargetableReciever
{
    void RecieveTargetableEntry(IEntityTargetAbleComponent targetable);
    void RecieveTargetableExit(IEntityTargetAbleComponent targetable);
}