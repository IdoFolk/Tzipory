using System;
using System.Collections.Generic;
using Shamans;
using Systems.TargetingSystem;
using Tools.Enums;
using Tzipory.EntitySystem.StatusSystem;
using Tzipory.EntitySystem;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

//TEMP NAME! BAD NAME!
public class PowerStructure : BaseGameEntity , ITargetableReciever 
{
    //temp config stuff
    [SerializeField] private Stat _range;
    
    [SerializeField] private StatusEffectConfig _myEffect;
    [SerializeField] private ProximityIndicatorHandler _proximityIndicatorHandler;
    [SerializeField] private Color _activeColor;

    private Dictionary<int, IDisposable> _activeStatusEffectOnShaman;

    protected override void Awake()
    {
        base.Awake();
        _activeStatusEffectOnShaman = new Dictionary<int, IDisposable>();
        _proximityIndicatorHandler.Init(_range.CurrentValue); 
    }

    private void OnDisable()
    {
        _proximityIndicatorHandler.Disable();
    }
    
    public void RecieveCollision(Collider2D other, IOStatType ioStatType)
    {
        switch (ioStatType)
        {
            case IOStatType.In:
                if(other.gameObject.CompareTag("ShadowShaman")) 
                    _proximityIndicatorHandler.ChangeColor(_activeColor);
                break;
            case IOStatType.Out:
                if (other.gameObject.CompareTag("ShadowShaman"))
                    _proximityIndicatorHandler.ResetColor();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(ioStatType), ioStatType, null);
        }
    }

    public void RecieveTargetableEntry(IEntityTargetAbleComponent targetable)
    {
        if (targetable is not Shaman shaman) return;
        
        Debug.Log($"ENTER {shaman.name}");

        if (_activeStatusEffectOnShaman.ContainsKey(shaman.EntityInstanceID))//temp!!!
            return;

        Debug.Log($"{shaman.name} entered the area of influence of {name}");
        IDisposable disposable = shaman.StatusHandler.AddStatusEffect(_myEffect);
        _activeStatusEffectOnShaman.Add(shaman.
            EntityInstanceID, disposable);
    }

    public void RecieveTargetableExit(IEntityTargetAbleComponent targetable)
    {
        if (targetable is not Shaman shaman) return;
        
        Debug.Log($"EXIT {shaman.name}");
            
        if (_activeStatusEffectOnShaman.TryGetValue(shaman.EntityInstanceID,out IDisposable disposable))
        {
            disposable.Dispose();
            _activeStatusEffectOnShaman.Remove(shaman.EntityInstanceID);
        }
    }
}