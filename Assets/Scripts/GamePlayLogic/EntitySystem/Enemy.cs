using System;
using System.Collections;
using System.Collections.Generic;
using Tzipory.GamePlayLogic.EntitySystem;
using Tzipory.Systems.PoolSystem;
using UnityEngine;

public class Enemy : UnitEntity , IPoolable<Enemy>
{
    

    public event Action<Enemy> OnDispose;
}
