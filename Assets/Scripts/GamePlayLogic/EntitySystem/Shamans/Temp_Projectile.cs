using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

public class Temp_Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _midDisToDeadTarget;
    
    private ITargetAbleEntity _target;
    private float _speed;

    private float _damage;
    private float _timeToDie;

    private bool _isCrit;
    private int _casterId;

    private Vector3 _dir;
    private Vector3 _lastTargetPosition;

    private bool _hitTarget;

    
    public void Init(BaseGameEntity baseGameEntity,ITargetAbleEntity target,float speed,float damage,float timeToDie,bool isCrit)
    {
        _timeToDie = timeToDie;
        _speed = speed;
        _target = target;
        _damage = damage;
        _isCrit = isCrit;
        _dir = (_target.GameEntity.transform.position - baseGameEntity.EntityTransform.position).normalized;
        transform.up = _dir;
        _lastTargetPosition = _target.GameEntity.transform.position;
        _hitTarget = false;
    }

    void Update()
    {
        _particleSystem.playbackSpeed = 1 * GAME_TIME.GetCurrentTimeRate;
        
        if (Vector2.Distance(_lastTargetPosition,transform.position) < _midDisToDeadTarget)
            _timeToDie = 0;
        else
            _timeToDie -= GAME_TIME.GameDeltaTime;
        
        transform.position += _dir * (_speed * GAME_TIME.GameDeltaTime);
        
        if (_timeToDie  <= 0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<ITargetAbleEntity>(out var hitedTarget))
        {
            if (hitedTarget.EntityType == EntityType.Hero) return;

            if (!_hitTarget)
            {
                hitedTarget.EntityHealthComponent.TakeDamage(_damage,_isCrit);
                _hitTarget = true;
            }
            
            Destroy(gameObject);
        }
    }
}