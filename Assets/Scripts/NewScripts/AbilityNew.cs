using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Tzipory.GamePlayLogic.EntitySystem; 

public abstract class AbilityNew : ScriptableObject
{
    [SerializeField] private float coolDown;

    public float CoolDown { get => coolDown; }

    public abstract void CastAbility(UnitEntity caster);
}
