using System;
using System.Collections.Generic;
using Shamans;
using Tzipory.EntitySystem.StatusSystem;
using Tzipory.EntitySystem;
using UnityEngine;

//TEMP NAME! BAD NAME!
public class PowerStructure : BaseGameEntity
{
    
    //temp config stuff
    [SerializeField] private float _range;


    [SerializeField] private Collider2D effectArea;
    [SerializeField] private StatusEffectConfigSo myEffectSO;
    [SerializeField] private ProximityIndicatorHandler _proximityIndicatorHandler;
    [SerializeField] private Color activeColor;

    private Dictionary<int, IDisposable> _activeStatusEffectOnShaman;

    protected override void Awake()
    {
        base.Awake();
        _activeStatusEffectOnShaman = new Dictionary<int, IDisposable>();
        //_proximityIndicatorHandler.Init(effectArea.radius*2f); //MAY need to move to OnEnable - especially if we use ObjectPooling instead of instantiate
        _proximityIndicatorHandler.Init(_range); 
    }

    private void OnDisable()
    {
        _proximityIndicatorHandler.Disable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shaman"))
        {

            Shaman shaman = collision.GetComponentInParent<Shaman>();
            Debug.LogWarning($"ENTER {shaman.name}");

            if (_activeStatusEffectOnShaman.ContainsKey(shaman.EntityInstanceID))//temp!!!
                return;

            Debug.Log($"{shaman.name} entered the area of influence of {name}");
            IDisposable disposable = shaman.StatusHandler.AddStatusEffect(myEffectSO);
            _activeStatusEffectOnShaman.Add(shaman.
                EntityInstanceID, disposable);
        }
        else if(collision.gameObject.CompareTag("ShadowShaman"))
        {
            _proximityIndicatorHandler.ChangeColor(activeColor);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shaman"))
        {
            Shaman shaman = collision.GetComponentInParent<Shaman>();
            Debug.LogWarning($"EXIT {shaman.name}");

            if (shaman is null)
                return;

            if (_activeStatusEffectOnShaman.TryGetValue(shaman.EntityInstanceID,out IDisposable disposable))
            {
                disposable.Dispose();
                _activeStatusEffectOnShaman.Remove(shaman.EntityInstanceID);
            }
        }
        else if (collision.gameObject.CompareTag("ShadowShaman"))
        {
            _proximityIndicatorHandler.ResetColor();
        }
    }

}
