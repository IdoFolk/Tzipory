using System;
using Tzipory.Systems.PoolSystem;
using UnityEngine;

public class Shaman : UnitEntity , IPoolable<Shaman>
{
    
    public event Action<Shaman> OnDispose;
}
