using System.Collections;
using System.Collections.Generic;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

public class EllipseTargetingArea : MonoBehaviour
{
    public List<IEntityTargetAbleComponent> GetInsiders => _insiders;
    List<IEntityTargetAbleComponent> _insiders = new List<IEntityTargetAbleComponent>();
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<IEntityTargetAbleComponent>(out var targetAbleComponent)) return;
        _insiders.Add(targetAbleComponent);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.TryGetComponent<IEntityTargetAbleComponent>(out var targetAbleComponent)) return;
        _insiders.Remove(targetAbleComponent);
    }

}
