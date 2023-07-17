using UnityEngine;

public class ShamanSpawnPoint : MonoBehaviour
{
    void Awake()
    { 
        GameManager.PartyManager.AddSpawnPoint(transform.position);   
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position, 1);
    }
}
