using System.Collections;
using System.Collections.Generic;
using NavMeshPlus.Components;
using UnityEngine;

public class NavMeshHelper : MonoBehaviour
{
    [SerializeField] private NavMeshSurface _navMeshSurface;
    
    void Start()
    {
        _navMeshSurface.BuildNavMesh();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
