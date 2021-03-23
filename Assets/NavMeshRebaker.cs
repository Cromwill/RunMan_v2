using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshRebaker : MonoBehaviour
{
    private NavMeshSurface _navMeshSurface;

    void Start()
    {
        _navMeshSurface = GetComponent<NavMeshSurface>();
    }

    public void Rebake()
    {
        _navMeshSurface.BuildNavMesh();
    }
}
