using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Tools.TimeSystem;
using UnityEngine;

public class Temp_Projectile : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _midDisToDeadTarget;
    
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
        _dir = (_target.EntityTransform.position - transform.position).normalized;
        transform.up = _dir;
    }

    void Update()
    {
        var lastTargetPosition = _target.EntityTransform.position;
        
        _particleSystem.playbackSpeed = 1 * GAME_TIME.GetCurrentTimeRate;

        if (_target.IsEntityDead)
            if (Vector2.Distance(lastTargetPosition,transform.position) < _midDisToDeadTarget)
                _timeToDie = 0;
            else
                _timeToDie -= GAME_TIME.GameDeltaTime;
        else
            _dir = (lastTargetPosition - transform.position).normalized;
        
        transform.position += _dir * (_speed * GAME_TIME.GameDeltaTime);
        
        if (_timeToDie  <= 0f)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<IEntityTargetAbleComponent>(out var hitedTarget))
        {
            if (hitedTarget.EntityType is EntityType.Hero or EntityType.Totem) return;
            //if (target.EntityInstanceID == _casterId) return;
            
            hitedTarget.TakeDamage(_damage,_isCrit);
            Destroy(gameObject);
        }
    }
}