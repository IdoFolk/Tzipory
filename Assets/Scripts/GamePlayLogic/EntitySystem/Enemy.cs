using System;
using Tzipory.Systems.PoolSystem;
using UnityEngine;

public class Enemy : UnitEntity , IPoolable<Enemy>
{
   

    public event Action<Enemy> OnDispose;
}
