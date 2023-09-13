using System;
using System.Collections.Generic;
using Shamans;
using Sirenix.OdinInspector;
using Systems.TargetingSystem;
using Tools.Enums;
using Tzipory.ConfigFiles.VisualSystemConfig;
using Tzipory.EntitySystem;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

//TEMP NAME! BAD NAME!
public class TEMP_PowerStructure : BaseGameEntity , ITargetableReciever 
{
    //temp config stuff
    [SerializeField] private float _range;
    
    [SerializeField,AssetsOnly,Required] private StatusEffectConfig _myEffect;
    [SerializeField] private ColliderTargetingArea _colliderTargetingArea;
    [SerializeField] private ProximityIndicatorHandler _proximityIndicatorHandler;
    [SerializeField] private Color _activeColor;

    private Dictionary<int, IDisposable> _activeStatusEffectOnShaman;

    protected override void Awake()
    {
        base.Awake();
        _activeStatusEffectOnShaman = new Dictionary<int, IDisposable>();
        _colliderTargetingArea.Init(this);
        _proximityIndicatorHandler.Init(_range); 
    }

    private void OnDisable()
    {
        _proximityIndicatorHandler.Disable();
    }
    
    public void RecieveCollision(Collider2D other, IOType ioType)
    {
        switch (ioType)
        {
            case IOType.In:
                if(other.gameObject.CompareTag("ShadowShaman")) 
                    _proximityIndicatorHandler.ChangeColor(_activeColor);
                break;
            case IOType.Out:
                if (other.gameObject.CompareTag("ShadowShaman"))
                    _proximityIndicatorHandler.ResetColor();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(ioType), ioType, null);
        }
    }

    public void RecieveTargetableEntry(IEntityTargetAbleComponent targetable)
    {
        if (targetable is not Shaman shaman) return;
        
        if (_activeStatusEffectOnShaman.ContainsKey(shaman.EntityInstanceID))//temp!!!
            return;

        IDisposable disposable = shaman.StatusHandler.AddStatusEffect(_myEffect);
        _activeStatusEffectOnShaman.Add(shaman.
            EntityInstanceID, disposable);
    }

    public void RecieveTargetableExit(IEntityTargetAbleComponent targetable)
    {
        if (targetable is not Shaman shaman) return;
        
        if (_activeStatusEffectOnShaman.TryGetValue(shaman.EntityInstanceID,out IDisposable disposable))
        {
            disposable.Dispose();
            _activeStatusEffectOnShaman.Remove(shaman.EntityInstanceID);
        }
    }
}