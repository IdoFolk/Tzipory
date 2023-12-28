using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShamanAttackHandler : MonoBehaviour
{
    [SerializeField] private float attackInterval;
    private float lastAttacked;
    private Shaman holder;




    private void Attack(Vector2 direction)
    {
        if (Time.time - lastAttacked >= attackInterval)
        {
            lastAttacked = Time.time;
        }

        Projectile autoAttack =  GameManager.Instance.NewPoolManager.ShamanAutoAttackPool.GetPooledObject();
        autoAttack.transform.position = holder.transform.position;
        autoAttack.InitProjectile(holder, holder.EntityStatComponent.GetStat(Tzipory.Helpers.Consts.Constant.StatsId.AttackDamage).CurrentValue, false);

    }






}
