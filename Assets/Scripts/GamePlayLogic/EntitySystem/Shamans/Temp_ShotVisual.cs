using Tzipory.Systems.Entity.EntityComponents;
using UnityEngine;

public class Temp_ShotVisual : MonoBehaviour
{
    [SerializeField] private Transform _shotPosition;
    
    private float _projectileSpeed;
    private float _timeToDie; 
    private Temp_Projectile _projectile;
    
    public void Init(Temp_Projectile  projectile,float projectileSpeed,float timeToDie)
    {
        _projectile = projectile;
        _projectileSpeed = projectileSpeed;
        _timeToDie = timeToDie;
    }
    
    public void Shot(ITargetAbleEntity target,float damage,bool isCrit)
    {
        var projectile = Instantiate(_projectile, _shotPosition.position, Quaternion.identity,_shotPosition);
        projectile.Init(target,_projectileSpeed,damage,_timeToDie,isCrit);
    }
}