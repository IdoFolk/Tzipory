using Tzipory.AbilitiesSystem;
using Tzipory.AbilitiesSystem.AbilityEntity;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

public class AoeAbilityEntity : BaseAbilityEntity
{
    private float _duration;

    public void Init(IEntityTargetAbleComponent target, float radius, float duration,IAbilityExecutor abilityExecutor)
    {
        base.Init(target,abilityExecutor);
        
        _collider2D.radius = radius;
        _collider2D.isTrigger = true;
        _duration = duration;
        //base.statusEffect = statusEffect;

        visualTransform.localScale = new Vector3(radius * 2.5f, radius * 2.5f, 0);

        var colliders = Physics2D.OverlapCircleAll(transform.position, _collider2D.radius);
        
        foreach (var collider in colliders)
        {
            if (collider.isTrigger)
                continue;
            
            if (collider.TryGetComponent(out IEntityTargetAbleComponent entityTargetAbleComponent))
                Cast(entityTargetAbleComponent);
        }
    }

    protected override void Update()
    {
        base.Update();
        
        _duration -= GAME_TIME.GameDeltaTime;//need to be a timer
        
        if(_duration <= 0)
            Destroy(gameObject);//temp need to add pool
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.TryGetComponent<IEntityTargetAbleComponent>(out var targetAbleComponent)) return;
            
        if (targetAbleComponent.EntityInstanceID == _abilityExecutor.Caster.EntityInstanceID) return;
            
        _abilityExecutor.Execute(targetAbleComponent);
    }
}
