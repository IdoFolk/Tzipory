using System;
using Tzipory.ConfigFiles.AbilitySystem;
using Tzipory.Systems.Entity;
using Tzipory.Systems.Entity.EntityComponents;
using Tzipory.Systems.StatusSystem;
using Tzipory.Systems.VisualSystem.EffectSequenceSystem;
using Tzipory.Systems.VisualSystem.PopUpSystem;
using Tzipory.Tools.Interface;
using UnityEngine;
using UnityEngine.Playables;

public class UnitEntityVisualHandler : MonoBehaviour , IEntityVisualComponent , IInitialization<BaseUnitEntity,EffectSequenceConfig[]>
{
    [SerializeField] private SpriteRenderer _mainSprite;
    
    [SerializeField] private Transform _visualQueueEffectPosition;
    [SerializeField] private PlayableDirector _particleEffectPosition;
    
    public int EntityInstanceID { get; private set; }
    public Transform EntityTransform { get; private set; }
    public BaseGameEntity GameEntity { get; private set; }
    public bool IsInitialization { get; private set; }
    public PopUpTexter PopUpTexter { get; private set; }

    public EffectSequenceHandler EffectSequenceHandler { get; private set; }
    public SpriteRenderer SpriteRenderer => _mainSprite;
    public PlayableDirector ParticleEffectPlayableDirector => _particleEffectPosition;
    
    public void Init(BaseUnitEntity parameter,EffectSequenceConfig[] effectSequence)
    {
        EntityInstanceID = parameter.EntityInstanceID;
        GameEntity = parameter;
        EntityTransform = parameter.EntityTransform;
        
        PopUpTexter = new PopUpTexter(_visualQueueEffectPosition);
        
        EffectSequenceHandler = new EffectSequenceHandler(this,effectSequence);
        
        IsInitialization = true;
    }

    private void Update()
    {
        EffectSequenceHandler.UpdateEffectHandler();
    }

    public void StartTimeLineEffect(EffectOnEntityConfig config)
    {
        _particleEffectPosition.playableAsset = config.EntryTimeLine;
        _particleEffectPosition.Play();
        
    }

    public void ResetVisual()
    {
        throw new NotImplementedException();
    }
}
