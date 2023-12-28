using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolsManager : MonoBehaviour
{
    [SerializeField] private ProjectilePool piercingShotPool;

    public ProjectilePool PiercingShotPool { get => piercingShotPool;}
}
