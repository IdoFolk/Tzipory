using NavMeshPlus.Components;
using UnityEngine;

namespace Tzipory.Helpers
{
    public class NavMeshHelper : MonoBehaviour
    {
        [SerializeField] private NavMeshSurface _navMeshSurface;

        void Start()=>
            _navMeshSurface.BuildNavMesh();
    }
}