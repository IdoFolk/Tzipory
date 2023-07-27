using Tzipory.AbilitiesSystem;
using Tzipory.AbilitiesSystem.AbilityEntity;
using Tzipory.AbilitiesSystem.AbilityExecuteTypes;
using Tzipory.BaseSystem.TimeSystem;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

public class AoeAbilityEntity : BaseAbilityEntity, ITargetableReciever
{
    private float _duration;
    private AoeAbilityExecuter _aoeAbilityExecuter;

    //OLD INIT

    public void Init(IEntityTargetAbleComponent target, float radius, float duration,IAbilityExecutor abilityExecutor)
    {
        base.Init(target,abilityExecutor);
        
        //_collider2D.radius = radius;
        _collider2D.isTrigger = true;
        _duration = duration;
        //base.statusEffect = statusEffect;

        visualTransform.localScale = new Vector3(radius * 2.5f, radius * 2.5f, 0); //why *2.5?
        _collider2D.transform.localScale = new Vector3(radius * 2.5f, radius * 2.5f, 0); //why *2.5?

        var colliders = Physics2D.OverlapCircleAll(transform.position, _collider2D.transform.localScale.x);


        foreach (var collider in colliders)
        {
            if (collider.isTrigger)
                continue;
            
            if (collider.TryGetComponent(out IEntityTargetAbleComponent entityTargetAbleComponent))
                Cast(entityTargetAbleComponent);
        }
    }
    //GOOD INIT!
    public void InitSimple(IEntityTargetAbleComponent target, float radius, float duration, AoeAbilityExecuter abilityExecutor)
    {
        base.Init(target,abilityExecutor);
        _aoeAbilityExecuter = abilityExecutor;
        //_collider2D.radius = radius;
        _collider2D.isTrigger = true;
        _duration = duration;
        //base.statusEffect = statusEffect;

        visualTransform.localScale = new Vector3(radius , radius, 1); //why *2.5?
        _collider2D.transform.localScale = new Vector3(radius , radius, 1); //why *2.5?

    }

    public void RecieveTargetableEntry(IEntityTargetAbleComponent targetable)
    {
        _aoeAbilityExecuter.Execute(targetable);
    }

    public void RecieveTargetableExit(IEntityTargetAbleComponent targetable)
    {
        _aoeAbilityExecuter.ExecuteOnExit(targetable);
    }

    protected override void Update()
    {
        base.Update();
        
        _duration -= GAME_TIME.GameDeltaTime;//need to be a timer
        
        if(_duration <= 0)
            Destroy(gameObject);//temp need to add pool
    }
}
