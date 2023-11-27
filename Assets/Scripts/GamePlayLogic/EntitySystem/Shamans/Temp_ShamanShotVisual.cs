using Tzipory.Systems.Entity.EntityComponents;
using UnityEngine;

public class Temp_ShamanShotVisual : MonoBehaviour
{
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private float _timeToDie;
    [SerializeField] private Temp_Projectile _projectile;
    [SerializeField] private Transform _parent;
    
    private Transform  _shotPosition;
    
    public void Init(Transform shotPosition)
    {
        _shotPosition = shotPosition;
    }
    
    public void Shot(ITargetAbleEntity target,float damage,bool isCrit)
    {
        var projectile = Instantiate(_projectile, _shotPosition.position, Quaternion.identity,_parent);
        projectile.Init(target,_projectileSpeed,damage,_timeToDie,isCrit);
    }
}