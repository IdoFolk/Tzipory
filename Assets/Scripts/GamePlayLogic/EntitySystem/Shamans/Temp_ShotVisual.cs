using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using UnityEngine;

public class Temp_ShotVisual : MonoBehaviour
{
    [SerializeField] private Transform _shotPosition;
    
    private float _projectileSpeed;
    private float _timeToDie; 
    private Temp_Projectile _projectile;

    private BaseGameEntity _baseGameEntity;
    
    public void Init(BaseGameEntity baseGameEntity,Temp_Projectile projectile,float projectileSpeed,float timeToDie)
    {
        _projectile = projectile;
        _projectileSpeed = projectileSpeed;
        _timeToDie = timeToDie;
        _baseGameEntity = baseGameEntity;
    }
    
    public void Shot(ITargetAbleEntity target,float damage,bool isCrit)
    {
        var projectile = Instantiate(_projectile, _shotPosition.position, Quaternion.identity,_shotPosition);
        projectile.Init(_baseGameEntity,target,_projectileSpeed,damage,_timeToDie,isCrit);
    }
}