using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

public class Temp_Projectile : MonoBehaviour
{
    private IEntityTargetAbleComponent _target;
    private float _speed;

    private float _damage;
    private float _timeToDie;

    private bool _isCrit;
    private int _casterId;

    private Vector3 _dir;

    
    public void Init(IEntityTargetAbleComponent target,float speed,float damage,float timeToDie,bool isCrit)
    {
        _timeToDie = timeToDie;
        _speed = speed;
        _target = target;
        _damage = damage;
        _isCrit = isCrit;
        _dir =(_target.EntityTransform.position - transform.position).normalized;
    }

    void Update()
    {
        if (!_target.EntityTransform.gameObject.activeInHierarchy)
        {
            transform.Translate(_dir * (_speed * GAME_TIME.GameDeltaTime));
            _timeToDie -= GAME_TIME.GameDeltaTime;
        }
        else
        {
            _dir =(_target.EntityTransform.position - transform.position).normalized;
            transform.position  += _dir * (_speed * GAME_TIME.GameDeltaTime);
        }

        if (_timeToDie  <= 0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IEntityTargetAbleComponent>(out var hitedTarget))
        {
            if (hitedTarget.EntityTeamType == EntityTeamType.Hero) return;
            //if (target.EntityInstanceID == _casterId) return;
            
            hitedTarget.TakeDamage(_damage,_isCrit);
            Destroy(gameObject);
        }
    }
}