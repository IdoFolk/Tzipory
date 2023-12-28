using UnityEngine;

public class PoolsManager : MonoBehaviour
{
    [SerializeField] private ProjectilePool piercingShotPool;
    [SerializeField] private ProjectilePool shamanAutoAttackPool;

    public ProjectilePool PiercingShotPool { get => piercingShotPool; }
    public ProjectilePool ShamanAutoAttackPool { get => shamanAutoAttackPool; }
}
