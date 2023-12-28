using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tzipory.Systems.Entity.EntityComponents;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private int maxNumberOfHits;
    [SerializeField] private bool limitedNumberOfHits;
    [SerializeField] private float baseDamage;
    [SerializeField] private Rigidbody2D rb;
    private int currentNumberOfHits = 0;
    private UnitEntity caster;
    private bool crit;

    private void OnEnable()
    {
        Invoke("Disable", lifeTime);
    }

    public void Fire(Vector2 direction)
    {
        rb.velocity = direction * speed;
    }


    public void InitProjectile(UnitEntity givenCaster, float baseDamage, bool critical)
    {
        caster = givenCaster;
        this.baseDamage = baseDamage;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //projectiles can only hit either enemies or shamans.
        UnitEntity targetHit = collision.gameObject.GetComponent<UnitEntity>();
        if (!ReferenceEquals(targetHit, null))
        {
            targetHit.EntityHealthComponent.TakeDamage(baseDamage, false);
        }
        if (limitedNumberOfHits)
        {
            currentNumberOfHits++;
            if (currentNumberOfHits >= maxNumberOfHits)
            {
                Disable();
            }
        }
    }

    private void Disable()
    {
        gameObject.SetActive(false);
        caster = null;
        crit = false;
        baseDamage = 0;
    }




}
