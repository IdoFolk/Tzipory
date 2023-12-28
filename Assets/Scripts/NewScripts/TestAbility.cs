using Tzipory.GameplayLogic.Managers.MainGameManagers;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "New Ability/Test")]
public class TestAbility : AbilityNew
{
    public override void CastAbility(UnitEntity caster)
    {
        if (ReferenceEquals(caster.EntityTargetingComponent.CurrentTarget, null))
        {
            return;
        }
        Projectile newProjectile = GameManager.Instance.NewPoolManager.PiercingShotPool.GetPooledObject();
        newProjectile.transform.position = caster.transform.position;
        newProjectile.gameObject.SetActive(true);
        newProjectile.Fire((caster.EntityTargetingComponent.CurrentTarget.GameEntity.transform.position - caster.transform.position).normalized);

    }
}
