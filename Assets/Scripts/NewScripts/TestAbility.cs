using Tzipory.GameplayLogic.Managers.MainGameManagers;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "New Ability/Test")]
public class TestAbility : AbilityNew
{
    public override void CastAbility(UnitEntity caster)
    {
        if (ReferenceEquals(((Shaman)caster).EnemyTargeter.ClosestTarget, null))
        {
            return;
        }
        Projectile newProjectile = GameManager.Instance.NewPoolManager.PiercingShotPool.GetPooledObject();
        newProjectile.InitProjectile(caster, 100, false);//temp
        newProjectile.transform.position = caster.transform.position;
        newProjectile.gameObject.SetActive(true);
        newProjectile.Fire((((Shaman)caster).EnemyTargeter.ClosestTarget.transform.position - caster.transform.position).normalized);//for now:)

    }
}
