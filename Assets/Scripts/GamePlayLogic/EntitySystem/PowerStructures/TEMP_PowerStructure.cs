using System;
using System.Collections.Generic;
using Shamans;
using Sirenix.OdinInspector;
using Systems.TargetingSystem;
using Tools.Enums;
using Tzipory.EntitySystem.StatusSystem;
using Tzipory.EntitySystem;
using Tzipory.EntitySystem.EntityComponents;
using UnityEngine;

//TEMP NAME! BAD NAME!
public class TEMP_PowerStructure : BaseGameEntity , ITargetableReciever 
{
    //temp config stuff
    [SerializeField] private float _range;
    //Temp also, this should be a Gradient struct
    [SerializeField] private float[] _ranges;
    [SerializeField] private float[] _values;
    //Gradient sturct please

    [SerializeField,AssetsOnly,Required] private StatusEffectConfig _myEffect;
    [SerializeField] private ColliderTargetingArea _colliderTargetingArea;
    [SerializeField] private ColliderTargetingArea[] _colliderTargetingAreas;
    [SerializeField] private RingedProximityIndicatorHandler _ringedProximityIndicatorHandler;
    [SerializeField] private Color _activeColor;

    private Dictionary<int, IDisposable> _activeStatusEffectOnShaman;

    protected override void Awake()
    {
        base.Awake();
        _activeStatusEffectOnShaman = new Dictionary<int, IDisposable>();
        foreach (var colliderTargetArea in _colliderTargetingAreas)
        {
            colliderTargetArea.Init(this); //this DOES NOT scale the colliders!
        }

        //_proximityIndicatorHandler.Init(_range); //this scales the gfx elements, and subs to events
        _ringedProximityIndicatorHandler.InitWithRanges(_ranges);
    }

    private void OnDisable()
    {
        _ringedProximityIndicatorHandler.Disable();
    }
    
    public void RecieveCollision(Collider2D other, IOStatType ioStatType)
    {
        switch (ioStatType)
        {
            case IOStatType.In:
                if(other.gameObject.CompareTag("ShadowShaman")) 
                    _ringedProximityIndicatorHandler.ChangeColor(_activeColor);
                break;
            case IOStatType.Out:
                if (other.gameObject.CompareTag("ShadowShaman"))
                    _ringedProximityIndicatorHandler.ResetColor();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(ioStatType), ioStatType, null);
        }
    }

    public void RecieveTargetableEntry(IEntityTargetAbleComponent targetable)
    {
        if (targetable is not Shaman shaman) return;
        
        Debug.Log($"ENTER {shaman.name}");
        IDisposable disposable;
        //if (_activeStatusEffectOnShaman.ContainsKey(shaman.EntityInstanceID))//temp!!!
        if (_activeStatusEffectOnShaman.TryGetValue(shaman.EntityInstanceID, out  disposable))//temp!!!
        {
            SetValueByDistance(shaman, disposable);
            return;
        }

        Debug.Log($"{shaman.name} entered the area of influence of {name}");
        disposable = shaman.StatusHandler.AddStatusEffect(_myEffect);
        _activeStatusEffectOnShaman.Add(shaman.
            EntityInstanceID, disposable);
        SetValueByDistance(shaman, disposable);
    }

    private void SetValueByDistance(Shaman shaman, IDisposable disposable)
    {
        //VECTOR2!!! so we avoid the distnace created by z axis - may be simpler to check distance between
        //the grpahic elemnets and the actual game objects themselves
        float distnace = Vector2.Distance(shaman.transform.position, transform.position) / 2;
        int statusEffectValueIndex = -1;

        if (distnace > _ranges[0])
        {
            DoTheDispose(shaman, disposable);
            //dispose!
            return;
        }
        else if (distnace <= _ranges[0] && distnace > _ranges[1])
        {
            //set to values[0]
            statusEffectValueIndex = 0;
        }
        else if (distnace <= _ranges[1] && distnace > _ranges[2])
        {
            //set to values[1]
            statusEffectValueIndex = 1;

        }
        else
        {
            statusEffectValueIndex = 2;
            //less or equal to the shortest range!
            //set values[2]
        }

        if (statusEffectValueIndex == -1)
        {
            Debug.LogError("Shouldnt ever happen");
            return;
        }
                //Set intensity to relevant distance, or remove if out of range.
                (disposable as BaseStatusEffect).SetMyFirstModifier(_values[statusEffectValueIndex]);
        return;
    }

    public void RecieveTargetableExit(IEntityTargetAbleComponent targetable)
    {
        if (targetable is not Shaman shaman) return;
        
        Debug.Log($"EXIT {shaman.name}");
            
        if (_activeStatusEffectOnShaman.TryGetValue(shaman.EntityInstanceID,out IDisposable disposable))
        {
            ////Distance check and dispose only if need be!
            //DoTheDispose(shaman, disposable);
            SetValueByDistance(shaman, disposable);
        }
    }

    private void DoTheDispose(Shaman shaman, IDisposable disposable)
    {
        disposable.Dispose();
        _activeStatusEffectOnShaman.Remove(shaman.EntityInstanceID);
    }
}