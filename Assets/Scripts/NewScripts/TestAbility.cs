using System.Collections;
using System.Collections.Generic;
using Tzipory.GamePlayLogic.EntitySystem;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "New Ability/Test")]
public class TestAbility : AbilityNew
{
    public override void CastAbility(UnitEntity caster)
    {
        Debug.Log($"{caster} is casting ability");
    }
}
