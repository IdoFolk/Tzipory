using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ProximityTargeter<T> : MonoBehaviour where T : Component
{
    //inherit this and set type to any type you wish to find? 

    private List<T> targetsInRange = new List<T>();
    private UnitEntity holder;

    public T ClosestTarget => GetClosestTarget();

    public virtual void Init(UnitEntity givenHolder)
    {
        holder = givenHolder;
    }


    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        T possibleTarget = collision.GetComponent<T>();
        if (!ReferenceEquals(possibleTarget, null))
        {
            targetsInRange.Add(possibleTarget);
            Debug.Log("added target");
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        T possibleTarget = collision.GetComponent<T>();
        if (!ReferenceEquals(possibleTarget, null))
        {
            targetsInRange.Remove(possibleTarget);
            Debug.Log("removed target");

        }
    }


    protected virtual T GetClosestTarget()
    {
        if (targetsInRange.Count <= 0)
        {
            return null;
        }

        T closestTarget = targetsInRange[0];
        float closestDist = Vector3.Distance(transform.position, targetsInRange[0].transform.position);
        foreach (var item in targetsInRange)
        {
            if (Vector3.Distance(transform.position, item.transform.position) <= closestDist)
            {
                closestTarget = item;
            }
        }
        return closestTarget;
    }

}
