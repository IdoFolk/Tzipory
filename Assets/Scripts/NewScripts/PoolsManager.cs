using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolsManager : MonoBehaviour
{
    [SerializeField] private ProjectilePool PiercingShotPool;

    public ProjectilePool PiercingShotPool1 { get => PiercingShotPool;}
}
