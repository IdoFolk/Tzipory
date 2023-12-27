using System;
using Tzipory.Systems.PoolSystem;
using UnityEngine;

public class Enemy : UnitEntity , IPoolable<Enemy>
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Core"))
        { 
            EntityHealthComponent.StartDeathSequence();
        }
    }

    public event Action<Enemy> OnDispose;
}
