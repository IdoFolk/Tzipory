using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreKillCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject.GetComponent<UnitEntity>();
            enemy.EntityHealthComponent.StartDeathSequence();
        }
    }
}
