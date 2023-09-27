using Tzipory.GameplayLogic.Managers.CoreGameManagers;
using UnityEngine;

namespace Tzipory.GameplayLogic.EntitySystem.Shamans
{
    public class ShamanSpawnPoint : MonoBehaviour
    {
        void Awake()
        {
            LevelManager.PartyManager.AddSpawnPoint(transform.position);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position, 1);
        }
    }
}