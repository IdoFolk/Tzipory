using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

public class Temp_ShamanShotVisual : MonoBehaviour
{
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _timeToDie;
    [SerializeField] private Temp_Projectile _projectile;
    
    public void Shot(IEntityTargetAbleComponent target,float damage,bool isCrit)
    {
        var projectile = Instantiate(_projectile, transform.position, Quaternion.identity);
        projectile.Init(target,_projectileSpeed,damage,_timeToDie,isCrit);
    }
}
